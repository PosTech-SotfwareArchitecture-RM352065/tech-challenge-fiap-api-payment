using System;

namespace Sanduba.Core.Application.Abstraction.Orders.Events
{
    public record OrderRejectedEvent(Guid OrderId);
}
