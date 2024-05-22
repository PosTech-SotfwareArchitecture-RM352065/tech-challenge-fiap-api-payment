using Sanduba.Core.Domain.Commons.Types;
using Sanduba.Core.Domain.Orders;
using System;
using System.Collections.Generic;

namespace Sanduba.Core.Domain.Payments
{
    public sealed class Payment : Entity<Guid>
    {
        public Payment(Guid id) : base(id) { }
        public Status Status { get; private set; }
        public Method Method { get; init; }
        public Provider Provider { get; init; } = Provider.MercadoPago;
        public string ExternalId { get; private set; } = null;
        public string QrData { get; private set; } = null;
        public ExternalPaymentProvider ExternalPaymentProvider { get; private set; }
        public Order Order { get; init; }

        public static Payment CreatePayment(Guid id, Order order, Method method)
        {
            var payment = new Payment(id)
            {
                Order = order,
                Status = Status.Created,
                Provider = Provider.MercadoPago,
                ExternalPaymentProvider = new ExternalPaymentProvider()
                {
                    ExternalProvider = Provider.MercadoPago
                },
                Method = method
            };

            payment.ValidateEntity();
            return payment;
        }

        public void SentToExternalPaymentProvider(string externalId, string qrData)
        {
            ExternalId = externalId;
            QrData = qrData;
            ExternalPaymentProvider.UpdateExternalPaymentData(externalId, qrData);
            Status = Status.WaitingPayment;
        }

        public void Cancelled()
        {
            Status = Status.Cancelled;
        }

        public void Payed()
        {
            Status = Status.Payed;
        }

        public override void ValidateEntity()
        {
            Order.ValidateEntity();
        }
    }
}
