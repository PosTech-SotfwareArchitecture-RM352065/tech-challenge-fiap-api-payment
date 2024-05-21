using MongoDB.Driver;
using Sanduba.Core.Application.Payments;
using Sanduba.Core.Domain.Payments;
using Sanduba.Infrastructure.Persistence.MongoDb.Configurations;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Sanduba.Infrastructure.Persistence.MongoDb.Payments
{
    public class PaymentPersistenceGateway : IPaymentRepository
    {
        private readonly MongoDbContext _dbContext;
        public PaymentPersistenceGateway(MongoDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Task SaveAsync(Payment entity, CancellationToken cancellationToken)
        {
            return _dbContext.Payments.InsertOneAsync(entity, cancellationToken);
        }

        public Task DeleteAsync(Payment entity, CancellationToken cancellationToken)
        {
            return _dbContext.Payments.FindOneAndDeleteAsync(item => item.Id == entity.Id, null, cancellationToken);
        }

        public async Task<IEnumerable<Payment>> GetAllAsync(CancellationToken cancellationToken)
        {
            var results = await _dbContext.Payments.FindAsync(_ => true);
            return results.ToList();
        }

        public async Task<IEnumerable<Payment>> GetAllAsync(Expression<Func<Payment, bool>> predicate, CancellationToken cancellationToken)
        {
            var results = await _dbContext.Payments.FindAsync(predicate);
            return results.ToList();
        }

        public async Task<IEnumerable<Payment>> GetAllPending(CancellationToken cancellationToken)
        {
            var results = await _dbContext.Payments.FindAsync(payment => payment.Status == Status.WaitingPayment);
            return results.ToList();
        }

        public async Task<Payment> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var results = await _dbContext.Payments.FindAsync(payment => payment.Id == id);
            return results.FirstOrDefault();
        }

        public async Task<IEnumerable<Payment>> GetByUserId(Guid id, CancellationToken cancellationToken)
        {
            var results = await _dbContext.Payments.FindAsync(payment => payment.Order.ClientId == id);
            return results.ToList();
        }

        public Task UpdateAsync(Payment entity, CancellationToken cancellationToken)
        {
            var filter = Builders<Payment>.Filter.Eq("Id", entity.Id);

            return _dbContext.Payments.ReplaceOneAsync(filter, entity, new ReplaceOptions { IsUpsert = true }, cancellationToken);
        }

        public async Task<Payment> GetByExternalProviderId(long id, CancellationToken cancellationToken)
        {
            var results = await _dbContext.Payments.FindAsync(payment => payment.ExternalId == id.ToString());
            return results.FirstOrDefault();
        }
    }
}
