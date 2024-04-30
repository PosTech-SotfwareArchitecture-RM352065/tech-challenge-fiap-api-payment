using Sanduba.Core.Domain.Commons.Types;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Sanduba.Core.Application.Commons
{
    public interface IAsyncRepository<TId, T> where T : Entity<TId>
    {
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);
        Task<T> GetByIdAsync(TId id, CancellationToken cancellationToken);
        Task SaveAsync(T entity, CancellationToken cancellationToken);
        Task UpdateAsync(T entity, CancellationToken cancellationToken);
        Task DeleteAsync(T entity, CancellationToken cancellationToken);
    }
}
