using Sanduba.Core.Domain.Commons.Assertions;
using Sanduba.Core.Domain.Commons.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sanduba.Core.Domain.Orders
{
    public sealed class Order : Entity<Guid>
    {
        public Order(Guid id) : base(id) { }

        public string Code { get; set; }

        public Guid ClientId { get; set; }

        public List<OrderItem> Items { get; set; }

        public double TotalAmount => Items.Sum(item => item.Amount);

        public override void ValidateEntity()
        {
            AssertionConcern.AssertArgumentNotEmpty(Items, "Pedido deve conter os itens!");
            Items.ForEach(item => item.Product.ValidateEntity());

        }
    }
}
