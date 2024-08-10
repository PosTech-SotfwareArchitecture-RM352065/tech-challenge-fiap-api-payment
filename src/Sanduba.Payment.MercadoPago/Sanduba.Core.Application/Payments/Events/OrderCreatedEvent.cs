using System;

namespace Sanduba.Core.Application.Abstraction.Orders.Events
{
    public record OrderCreatedEvent(Guid OrderId);
}
