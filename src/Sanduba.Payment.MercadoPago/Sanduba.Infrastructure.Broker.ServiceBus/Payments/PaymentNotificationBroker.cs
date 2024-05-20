using MassTransit;
using Microsoft.Extensions.Options;
using Sanduba.Core.Application.Payments;
using Sanduba.Core.Application.Payments.ResponseModel.ExternalProvider;
using Sanduba.Infrastructure.Broker.ServiceBus.Configurations.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sanduba.Infrastructure.Broker.ServiceBus.Payments
{
    public class PaymentNotificationBroker(IPublishEndpoint publishClient, IOptions<ServiceBusOptions> options) : IPaymentNotification
    {
        private readonly IPublishEndpoint _publishClient = publishClient;

        public async Task UpdatedPayment(PaymentDetailData requestModel, CancellationToken cancellationToken)
        {
            await _publishClient.Publish<PaymentDetailData>(requestModel, cancellationToken);
        }
    }
}
