using Sanduba.Core.Domain.Commons.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sanduba.Core.Domain.Orders
{
    public sealed class Order : Entity<Guid>
    {
        public Order(Guid id) : base(id) { }

        public string Code { get; init; }

        public Guid ClientId { get; init; }

        public List<OrderItem> Items { get; init; }

        public double TotalAmount => Items.Sum(item => item.Amount);

        public override void ValidateEntity()
        {
            throw new NotImplementedException();
        }
    }
}
