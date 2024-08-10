using Sanduba.Core.Domain.Payments;
using System;


namespace Sanduba.Core.Application.Abstraction.Orders.Events
{
    public record OrderPaymentConfirmedEvent(Guid OrderId, Guid PaymentId, Method Method, Provider Provider);
}
