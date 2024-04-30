using Sanduba.Core.Application.Commons;
using Sanduba.Core.Domain.Payments;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Sanduba.Core.Application.Payments
{
    public interface IPaymentRepository : IAsyncRepository<Guid, Payment>
    {
        public Task<IEnumerable<Payment>> GetByUserId(Guid id, CancellationToken cancellationToken);
        public Task<IEnumerable<Payment>> GetAllPending(CancellationToken cancellationToken);
        public Task<Payment> GetByExternalProviderId(long id, CancellationToken cancellationToken);
    }
}
