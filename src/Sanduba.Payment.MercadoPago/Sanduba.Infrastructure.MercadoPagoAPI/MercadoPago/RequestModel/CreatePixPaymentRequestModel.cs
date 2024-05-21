using Sanduba.Core.Domain.Payments;
using System;
using System.Linq;

namespace Sanduba.Infrastructure.MercadoPagoAPI.MercadoPago.RequestModel
{
    public record CreatePixPaymentRequestModel(
        string Description,
        string ExternalReference,
        OrderItem[] Items,
        string NotificationUrl,
        string Title,
        double TotalAmount,
        string ExpirationDate
    );

    public record OrderItem(
        string Category,
        string Title,
        string Description,
        double UnitPrice,
        double Quantity,
        string UnitMeasure,
        double TotalAmount
    );
}
