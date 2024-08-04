using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;


namespace Sanduba.Infrastructure.Persistence.MongoDb.Configurations.Options
{
    [ExcludeFromCodeCoverage]
    public class MongoConfigureOptions(IConfiguration configuration) : IConfigureOptions<MongoOptions>
    {
        private readonly string _connectionStringKey = "MongoSettings:ConnectionString";
        private readonly string _databaseNameKey = "MongoSettings:DatabaseName";
        private readonly string _collectionNameKey = "MongoSettings:CollectionName";
        private readonly IConfiguration _configuration = configuration;

        public void Configure(MongoOptions options)
        {
            options.ConnectionString = _configuration.GetValue<string>(_connectionStringKey) ?? string.Empty;
            options.DatabaseName = _configuration.GetValue<string>(_databaseNameKey) ?? string.Empty;
            options.CollectionName = _configuration.GetValue<string>(_collectionNameKey) ?? string.Empty;
        }
    }
}
