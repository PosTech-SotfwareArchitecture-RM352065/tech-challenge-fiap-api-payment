using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;

namespace Sanduba.Infrastructure.MercadoPagoAPI.Configurations.Options
{
    [ExcludeFromCodeCoverage]
    public class MercadoPagoConfigureOptions(IConfiguration configuration) : IConfigureOptions<MercadoPagoOptions>
    {
        private readonly string _baseUrl = "MercadoPagoSettings:BaseUrl";
        private readonly string _notificationUrl = "MercadoPagoSettings:NotificationUrl";
        private readonly string _authenticationToken = "MercadoPagoSettings:AuthenticationToken";
        private readonly string _userId = "MercadoPagoSettings:UserId";
        private readonly string _cashierId = "MercadoPagoSettings:CashierId";
        private readonly IConfiguration _configuration = configuration;

        public void Configure(MercadoPagoOptions options)
        {
            options.BaseUrl = _configuration.GetValue<string>(_baseUrl) ?? string.Empty;
            options.NotificationUrl = _configuration.GetValue<string>(_notificationUrl) ?? string.Empty;
            options.AutheticationToken = _configuration.GetValue<string>(_authenticationToken) ?? string.Empty;
            options.UserId = _configuration.GetValue<string>(_userId) ?? string.Empty;
            options.CashierId = _configuration.GetValue<string>(_cashierId) ?? string.Empty;
        }
    }
}