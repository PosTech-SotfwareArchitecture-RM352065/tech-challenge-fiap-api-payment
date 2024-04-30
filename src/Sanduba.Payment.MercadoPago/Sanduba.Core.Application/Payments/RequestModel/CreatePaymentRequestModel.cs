
using Sanduba.Core.Domain.Orders;
using Sanduba.Core.Domain.Payments;
using System;

namespace Sanduba.Core.Application.Payments.RequestModel
{
    public record CreatePaymentRequestModel (Order Order, Method Method, Provider Provider);
}
