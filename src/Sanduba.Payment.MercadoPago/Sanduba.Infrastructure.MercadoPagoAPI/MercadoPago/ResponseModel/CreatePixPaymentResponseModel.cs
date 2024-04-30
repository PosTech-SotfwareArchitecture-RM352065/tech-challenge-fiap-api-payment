namespace Sanduba.Infrastructure.MercadoPagoAPI.MercadoPago.ResponseModel
{
    public record CreatePixPaymentResponseModel(
        string InStoreOrderId,
        string QrData
    );
}
