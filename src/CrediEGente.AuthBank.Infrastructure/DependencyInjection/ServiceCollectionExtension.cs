using CapitalNerd.Laranjinhai.Domain.Interfaces;
using CapitalNerd.Laranjinhai.Infrastructure.Data;
using CapitalNerd.Laranjinhai.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CapitalNerd.Laranjinhai.Infrastructure.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddInfrastructureExtension(this IServiceCollection services, IConfiguration configuration)
        {
            // Repositórios
            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<IEmpresaRepository, EmpresaRepository>();
            services.AddScoped<IOperadorRepository, OperadorRepository>();
            services.AddScoped<IMensagemRepository, MensagemRepository>();
            services.AddScoped<IRespostaRepository, RespostaRepository>();
            services.AddScoped<ISessaoWhatsappRepository, SessaoWhatsappRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();


            // DbContext com PostgreSQL
            services.AddDbContext<LaranjinhaiDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            return services;
        }
    }
}
