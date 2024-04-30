using Sanduba.Core.Domain.Commons.Types;
using System.Collections.Generic;

namespace Sanduba.Core.Domain.Orders
{
    public sealed class OrderItem : ValueObject
    {
        public int Code { get; init; }
        public Product Product { get; init; }
        public Currency Currency { get; init; } = Currency.BRL;
        public double Amount { get; init; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Code;
        }
    }
}
