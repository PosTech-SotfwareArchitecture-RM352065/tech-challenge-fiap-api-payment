namespace Sanduba.Core.Domain.Payments
{
    public class ExternalPaymentProvider
    {
        public Provider ExternalProvider { get; init; }
        public string ExternalId { get; private set; }
        public string RawData { get; private set; }

        public void UpdateExternalPaymentData(string externalId, string rawData)
        {
            ExternalId = externalId;
            RawData = rawData;
        }
    }
}
