using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Sanduba.Core.Domain.Payments;
using Sanduba.Infrastructure.Persistence.MongoDb.Configurations.Options;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Sanduba.Infrastructure.Persistence.MongoDb.Configurations
{
    [ExcludeFromCodeCoverage]
    public class MongoDbContext
    {
        private readonly MongoOptions _options;
        private readonly IMongoDatabase _database;

        public MongoDbContext(IOptions<MongoOptions> options)
        {
            if (options is null) throw new ArgumentNullException(nameof(options));

            _options = options.Value;

            var client = new MongoClient(_options.ConnectionString);
            if (client != null)
            {
                _database = client.GetDatabase(_options.DatabaseName);
            }
        }

        public IMongoCollection<Payment> Payments
        {
            get
            {
                return _database.GetCollection<Payment>(_options.CollectionName);
            }
        }
    }
}
