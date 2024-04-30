using Sanduba.Core.Domain.Orders;
using Sanduba.Core.Domain.Payments;
using System;

namespace Sanduba.Core.Application.Payments.ResponseModel
{
    public record QueryPaymentByIdResponseModel (
        Guid Id,
        Status Status, 
        Order Order, 
        Method Method, 
        Provider Provider,
        string ExternalId,
        string QrData
    );
}
