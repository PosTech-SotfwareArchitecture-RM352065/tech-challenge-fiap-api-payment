using Sanduba.Core.Domain.Commons.Events;
using System;
using System.Collections.Generic;

namespace Sanduba.Core.Domain.Commons.Types
{
    public abstract class Entity<TId>
    {
        public TId Id { get; private init; }
        //public DateTime? CreatedAt { get; private set; }
        //public DateTime? LastModifiedAt { get; private set; }

        protected Entity(TId id)
        {
            Id = id;
        }

        public abstract void ValidateEntity();

        private readonly List<DomainEvent> _events = new();

        protected void RaiseDomainEvent(DomainEvent domainEvent)
        {
            _events.Add(domainEvent);
        }
    }
}
