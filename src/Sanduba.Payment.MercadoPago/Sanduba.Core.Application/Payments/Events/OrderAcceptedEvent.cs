using System;

namespace Sanduba.Core.Application.Abstraction.Orders.Events
{
    public record OrderAcceptedEvent(Guid OrderId);
}
