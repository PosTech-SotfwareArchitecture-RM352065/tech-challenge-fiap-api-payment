using Sanduba.Core.Application.Abstraction.Orders.Events;
using System.Threading;
using System.Threading.Tasks;

namespace Sanduba.Core.Application.Payments
{
    public interface IPaymentNotification
    {
        public Task UpdatedPayment(OrderPaymentConfirmedEvent eventData, CancellationToken cancellationToken);
        public Task UpdatedPayment(OrderPaymentRejectedEvent eventData, CancellationToken cancellationToken);
    }
}
