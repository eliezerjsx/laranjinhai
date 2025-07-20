using CapitalNerd.Laranjinhai.Application.Interfaces;
using CapitalNerd.Laranjinhai.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CapitalNerd.Laranjinhai.Infrastructure.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddApplicationExtension(this IServiceCollection services, IConfiguration configuration = default!)
        {
            services.AddScoped<IMensagemService, MensagemService>();

            return services;
        }
    }
}
