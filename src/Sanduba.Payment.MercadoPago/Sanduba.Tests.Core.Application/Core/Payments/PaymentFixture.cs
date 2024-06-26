﻿using Sanduba.Core.Application.Payments.RequestModel;
using Sanduba.Core.Domain.Orders;
using Sanduba.Core.Domain.Payments;
using System;
using System.Collections.Generic;
namespace Sanduba.Test.Unit.Core.Payments
{
    internal static class PaymentFixture
    {
        internal static Payment PaymentCreated(Guid paymentId)
        {
            return Payment.CreatePayment(
                paymentId,
                new Order(Guid.NewGuid())
                {
                    Code = "123456",
                    ClientId = Guid.NewGuid(),
                    Items = new List<OrderItem>
                    {
                        new OrderItem
                        {
                            Code = 1,
                            Product = new Product(Guid.NewGuid())
                            {
                                Name = "Product 1",
                                Description = "Product 1 description",
                                Category = Category.MainDish,
                                UnitPrice = 100.00
                            },
                            Currency = Currency.BRL,
                            Amount = 100.00
                        }
                    }
                },
                Method.PIX);
        }

        internal static Payment PaymentSentToProvider(Guid paymentId)
        {
            var payment = PaymentCreated(paymentId);
            payment.SentToExternalPaymentProvider("ExternalId", "QrCodeData");

            return payment;
        }

        internal static CreatePaymentRequestModel ValidPaymentCreationRequestModel()
        {
            return new CreatePaymentRequestModel
            (
                Order: new Order(Guid.NewGuid())
                {
                    Code = "123456",
                    ClientId = Guid.NewGuid(),
                    Items = new List<OrderItem>
                    {
                        new OrderItem
                        {
                            Product = new Product(Guid.NewGuid())
                            {
                                Name = "Product 1",
                                Description = "Product 1 description",
                                UnitPrice = 100.00
                            },
                            Currency = Currency.BRL,
                            Amount = 100.00
                        }
                    }

                },
                Method: Method.PIX,
                Provider: Provider.MercadoPago
            );
        }

        internal static CreatePaymentRequestModel InvalidPaymentCreationRequestModel()
        {
            return new CreatePaymentRequestModel
            (
                Order: new Order(Guid.NewGuid())
                {
                    Code = "123456",
                    ClientId = Guid.NewGuid(),
                    Items = new List<OrderItem>()

                },
                Method: Method.PIX,
                Provider: Provider.MercadoPago
            );
        }
    }
}
