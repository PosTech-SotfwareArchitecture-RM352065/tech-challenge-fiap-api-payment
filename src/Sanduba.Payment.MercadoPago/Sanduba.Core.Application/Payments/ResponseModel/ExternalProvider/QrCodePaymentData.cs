namespace Sanduba.Core.Application.Payments.ResponseModel.ExternalProvider
{
    public record QrCodePaymentData(
        string ExternalId,
        string QrCodeData
    );
}
