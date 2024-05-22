using System.Diagnostics.CodeAnalysis;

namespace Sanduba.Infrastructure.MercadoPagoAPI.Configurations.Options
{
    [ExcludeFromCodeCoverage]
    public class MercadoPagoOptions
    {
        public string BaseUrl { get; set; } = string.Empty;
        public string NotificationUrl { get; set; } = string.Empty;
        public string AutheticationToken { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string CashierId { get; set; } = string.Empty;
    }
}
