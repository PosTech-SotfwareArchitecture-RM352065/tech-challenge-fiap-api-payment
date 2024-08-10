using System;

namespace Sanduba.Core.Application.Abstraction.Orders.Events
{
    public record OrderPaymentRejectedEvent(Guid OrderId, string? Comments);
}
