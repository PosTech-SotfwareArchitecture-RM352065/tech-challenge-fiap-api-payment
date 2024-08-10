using System;

namespace Sanduba.Core.Application.Abstraction.Orders.Events
{
    public record OrderPreparationStartedEvent(Guid OrderId, DateTime DeliveryEstimation);
}
