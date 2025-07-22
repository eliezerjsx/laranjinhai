using System.Text.Json;
using CapitalNerd.Laranjinhai.API.Middlewares;
using CapitalNerd.Laranjinhai.Application.Commands;
using CapitalNerd.Laranjinhai.Application.Interfaces;
using CapitalNerd.Laranjinhai.Application.Services;
using CapitalNerd.Laranjinhai.Application.Settings;
using CapitalNerd.Laranjinhai.Domain.Interfaces;
using CapitalNerd.Laranjinhai.Infrastructure.Data;
using CapitalNerd.Laranjinhai.Infrastructure.DependencyInjection;
using CapitalNerd.Laranjinhai.Infrastructure.UnitOfWork;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

// OpenTelemetry Tracing
builder.Services.AddOpenTelemetry()
    .WithTracing(tracerProviderBuilder =>
        tracerProviderBuilder
            .AddSource("Laranjinhai.API")
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("Laranjinhai.API"))
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddConsoleExporter());


builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddValidatorsFromAssemblyContaining<CapitalNerd.Laranjinhai.Application.AssemblyReference>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Laranjinhai API", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token no formato: Bearer {seu_token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                },
                Array.Empty<string>()
            }
    });
});

builder.Services.AddHealthChecks();

builder.Services.AddInfrastructureExtension(builder.Configuration);
builder.Services.AddApplicationExtension(builder.Configuration);
builder.Services.Configure<OpenAISettings>(builder.Configuration.GetSection("OpenAI"));

builder.Services.Configure<WhatsAppSettings>(builder.Configuration.GetSection("WhatsApp"));

builder.Services.AddHttpClient<IWhatsAppSender, WhatsAppSender>((provider, client) =>
{
    var settings = provider.GetRequiredService<IOptions<WhatsAppSettings>>().Value;
    client.BaseAddress = new Uri(settings.BaseUrl);
    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {settings.ApiKey}");
});

builder.Services.Configure<RedisSettings>(builder.Configuration.GetSection("Redis"));

builder.Services.AddStackExchangeRedisCache(options =>
{
    var redisSettings = builder.Configuration.GetSection("Redis").Get<RedisSettings>();

    options.Configuration = $"{redisSettings.Host}:{redisSettings.Port}," +
                            $"password={redisSettings.Password}," +
                            $"ssl={redisSettings.Ssl.ToString().ToLower()}," +
                            $"abortConnect={redisSettings.AbortConnect.ToString().ToLower()}";

    options.InstanceName = "wpp:";
});

builder.Services.AddMediatR(cfg =>
        cfg.RegisterServicesFromAssembly(typeof(ProcessarMensagemCommand).Assembly));

builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policyBuilder =>
        policyBuilder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

var keycloakConfig = builder.Configuration.GetSection("Authentication:Keycloak").Get<KeycloakSettings>();

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = keycloakConfig.Authority;
        options.RequireHttpsMetadata = keycloakConfig.RequireHttpsMetadata;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudiences = keycloakConfig.Audiences
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("AllowAll");

app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/swagger");
        return;
    }

    await next();
});

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var result = JsonSerializer.Serialize(new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                error = e.Value.Exception?.Message
            })
        });
        await context.Response.WriteAsync(result);
    }
});

app.UseHttpsRedirection();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

if (app.Environment.IsDevelopment())
{
    await using var scope = app.Services.CreateAsyncScope();
    await using var dbContext = scope.ServiceProvider.GetRequiredService<LaranjinhaiDbContext>();
    await dbContext.Database.EnsureCreatedAsync();
}

if (app.Environment.IsProduction())
{
    using var scope = app.Services.CreateScope();
    var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("Startup");

    try
    {
        var db = scope.ServiceProvider.GetRequiredService<LaranjinhaiDbContext>();
        if (await db.Database.CanConnectAsync())
        {
            logger.LogInformation("Conexão com o banco de dados estabelecida com sucesso.");
        }
        else
        {
            logger.LogError("Não foi possível conectar ao banco de dados.");
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Falha ao conectar ao banco de dados.");
    }
}

app.Run();
