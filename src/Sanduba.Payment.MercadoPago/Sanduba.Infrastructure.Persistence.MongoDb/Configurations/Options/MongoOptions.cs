using System.Diagnostics.CodeAnalysis;

namespace Sanduba.Infrastructure.Persistence.MongoDb.Configurations.Options
{
    [ExcludeFromCodeCoverage]
    public class MongoOptions
    {
        public string ConnectionString = string.Empty;
        public string DatabaseName = string.Empty;
        public string CollectionName = string.Empty;
    }
}
