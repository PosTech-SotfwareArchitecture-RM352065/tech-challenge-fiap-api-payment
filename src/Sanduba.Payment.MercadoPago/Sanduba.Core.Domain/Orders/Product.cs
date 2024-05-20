using Sanduba.Core.Domain.Commons.Types;
using System;

namespace Sanduba.Core.Domain.Orders
{
    public sealed class Product : Entity<Guid>
    {
        public Product(Guid id) : base(id) { }

        public string Name { get; set; }

        public string Description { get; set; }

        public double UnitPrice { get; set; }

        public string Category { get; set; }

        public override void ValidateEntity()
        {
            throw new NotImplementedException();
        }
    }
}
