using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sanduba.Infrastructure.Broker.ServiceBus.Configurations;
using Sanduba.Infrastructure.MercadoPagoAPI.Configurations;
using Sanduba.Infrastructure.Persistence.MongoDb.Configurations;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sanduba.Cloud.Function.MercadoPago
{
    [ExcludeFromCodeCoverage]
    public static class Program
    {
        public static void Main()
        {
            var host = new HostBuilder()
            .ConfigureFunctionsWebApplication()
            .ConfigureServices((context, services) =>
            {

                var entryAssemblies = AppDomain.CurrentDomain.GetAssemblies();

                services.AddApplicationInsightsTelemetryWorkerService();
                services.ConfigureFunctionsApplicationInsights();
                services.AddMongoDbInfrastructure(context.Configuration);
                services.AddMercadoPagoInfrastructure(context.Configuration);
                services.AddServiceBusInfrastructure(context.Configuration);
                services.Configure<JsonSerializerOptions>(options =>
                {
                    options.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
                    options.PropertyNameCaseInsensitive = true;
                    options.WriteIndented = true;
                    options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
                });
                services.AddAutoMapper(entryAssemblies);
            })
            .Build();

            host.Run();
        }
    }
}