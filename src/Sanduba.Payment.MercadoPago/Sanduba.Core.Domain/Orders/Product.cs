using Sanduba.Core.Domain.Commons.Assertions;
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

        public Category Category { get; set; }

        public override void ValidateEntity()
        {
            AssertionConcern.AssertArgumentNotEmpty(Name, "Nome do produto não pode ser vazio!");
            AssertionConcern.AssertArgumentNotEmpty(Description, "Descrição do produto não pode ser vazio!");
            AssertionConcern.AssertArgumentNotNegative(UnitPrice, "Valor unitário não pode ser negativo!");
        }
    }
}
