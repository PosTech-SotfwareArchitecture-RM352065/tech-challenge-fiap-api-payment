using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sanduba.Infrastructure.MercadoPagoAPI.Configurations;
using Sanduba.Infrastructure.Persistence.MongoDb.Configurations;
using System.Text.Json;
using System.Text.Json.Serialization;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices((context, services) =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddMongoDbInfrastructure(context.Configuration);
        services.AddMercadoPagoInfrastructure(context.Configuration);
        services.Configure<JsonSerializerOptions>(options => {
            options.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
            options.PropertyNameCaseInsensitive = true;
            options.WriteIndented = true;
            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
        });
    })
    .Build();

host.Run();
