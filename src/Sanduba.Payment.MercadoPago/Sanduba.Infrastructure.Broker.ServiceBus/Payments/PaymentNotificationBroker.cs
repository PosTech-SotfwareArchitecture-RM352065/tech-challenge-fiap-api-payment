using MassTransit;
using Sanduba.Core.Application.Abstraction.Orders.Events;
using Sanduba.Core.Application.Payments;
using System.Threading;
using System.Threading.Tasks;

namespace Sanduba.Infrastructure.Broker.ServiceBus.Payments
{
    public class PaymentNotificationBroker(
        IPublishEndpoint publishClient) : IPaymentNotification
    {
        private readonly IPublishEndpoint _publishClient = publishClient;

        public async Task UpdatedPayment(OrderPaymentConfirmedEvent eventData, CancellationToken cancellationToken)
        {
            await _publishClient.Publish<OrderPaymentConfirmedEvent>(eventData, cancellationToken);
        }

        public async Task UpdatedPayment(OrderPaymentRejectedEvent eventData, CancellationToken cancellationToken)
        {
            await _publishClient.Publish<OrderPaymentRejectedEvent>(eventData, cancellationToken);
        }
    }
}
