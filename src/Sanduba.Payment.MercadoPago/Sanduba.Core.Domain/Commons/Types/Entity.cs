using System.Diagnostics.CodeAnalysis;

namespace Sanduba.Core.Domain.Commons.Types
{
    [ExcludeFromCodeCoverage]
    public abstract class Entity<TId>(TId id)
    {
        public TId Id { get; private init; } = id;

        public abstract void ValidateEntity();
    }
}
