using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sanduba.Core.Application.Payments;
using Sanduba.Infrastructure.Persistence.MongoDb.Configurations.Options;
using Sanduba.Infrastructure.Persistence.MongoDb.Payments;

namespace Sanduba.Infrastructure.Persistence.MongoDb.Configurations
{
    public static class DependencyInjection
    {
        /// <summary>
        /// Registers the necessary services with the DI framework.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>The same service collection.</returns>
        public static IServiceCollection AddMongoDbInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IPaymentRepository, PaymentPersistenceGateway>();
            services.AddSingleton<MongoDbContext>();
            services.AddOptions().ConfigureOptions<MongoConfigureOptions>();

            return services;
        }
    }
}