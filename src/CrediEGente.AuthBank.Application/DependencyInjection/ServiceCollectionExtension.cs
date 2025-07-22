using CapitalNerd.Laranjinhai.Application.Interfaces;
using CapitalNerd.Laranjinhai.Application.Services;
using CapitalNerd.Laranjinhai.Application.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CapitalNerd.Laranjinhai.Infrastructure.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddApplicationExtension(this IServiceCollection services, IConfiguration configuration = default!)
        {
            services.AddScoped<IMensagemService, MensagemService>();            
            services.AddHttpClient<IOpenAIService, OpenAIService>();
            services.AddScoped<ISessaoManager, SessaoManager>();

            return services;
        }
    }
}
