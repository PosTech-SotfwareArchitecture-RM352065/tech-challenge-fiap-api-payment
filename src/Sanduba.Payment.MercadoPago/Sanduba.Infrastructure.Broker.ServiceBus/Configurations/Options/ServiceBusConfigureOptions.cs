using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;


namespace Sanduba.Infrastructure.Broker.ServiceBus.Configurations.Options
{
    public class ServiceBusConfigureOptions(IConfiguration configuration) : IConfigureOptions<ServiceBusOptions>
    {
        private readonly string _connectionStringKey = "BrokerSettings:ConnectionString";
        private readonly string _topicNameKey = "BrokerSettings:TopicName";
        private readonly IConfiguration _configuration = configuration;

        public void Configure(ServiceBusOptions options)
        {
            options.ConnectionString = _configuration.GetValue<string>(_connectionStringKey) ?? string.Empty;
            options.TopicName = _configuration.GetValue<string>(_topicNameKey) ?? string.Empty;
        }
    }
}
