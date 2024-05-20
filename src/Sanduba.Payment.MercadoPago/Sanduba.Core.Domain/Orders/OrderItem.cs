using Sanduba.Core.Domain.Commons.Types;
using System.Collections.Generic;

namespace Sanduba.Core.Domain.Orders
{
    public sealed class OrderItem : ValueObject
    {
        public int Code { get; set; }
        public Product Product { get; set; }
        public Currency Currency { get; set; } = Currency.BRL;
        public double Amount { get; set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Code;
        }
    }
}
