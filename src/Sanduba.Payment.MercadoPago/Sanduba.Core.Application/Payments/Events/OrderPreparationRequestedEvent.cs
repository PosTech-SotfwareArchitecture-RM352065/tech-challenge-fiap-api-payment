﻿using System;

namespace Sanduba.Core.Application.Abstraction.Orders.Events
{
    public record OrderPreparationRequestedEvent(Guid OrderId);
}
