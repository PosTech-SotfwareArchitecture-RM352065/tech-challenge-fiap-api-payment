using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sanduba.Core.Application.Payments;
using Sanduba.Infrastructure.MercadoPagoAPI.Configurations.Options;
using Sanduba.Infrastructure.MercadoPagoAPI.MercadoPago;
using System.Diagnostics.CodeAnalysis;

namespace Sanduba.Infrastructure.MercadoPagoAPI.Configurations
{
    [ExcludeFromCodeCoverage]
    public static class DependencyInjection
    {
        /// <summary>
        /// Registers the necessary services with the DI framework.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>The same service collection.</returns>
        public static IServiceCollection AddMercadoPagoInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IPaymentExternalProvider, MercadoPagoApiGateway>();
            services.AddOptions().ConfigureOptions<MercadoPagoConfigureOptions>();

            return services;
        }
    }
}
