using System;
using Xunit;
using Moq;
using Sanduba.Core.Application.Payments;
using Sanduba.Core.Application.Payments.RequestModel;
using Sanduba.Core.Application.Payments.ResponseModel;
using Sanduba.Core.Domain.Payments;
using Sanduba.Core.Domain.Orders;
using System.Collections.Generic;

namespace Sanduba.Tests.Core.Application
{
    public class PaymentInteractorTest
    {
        private readonly Mock<IPaymentRepository> _paymentRepositoryMock;
        private readonly Mock<IPaymentExternalProvider> _externalProviderMock;
        private readonly Mock<IPaymentNotification> _paymentNotificationMock;
        private readonly PaymentInteractor _paymentInteractor;

        public PaymentInteractorTest()
        {
            _paymentRepositoryMock = new Mock<IPaymentRepository>();
            _externalProviderMock = new Mock<IPaymentExternalProvider>();
            _paymentNotificationMock = new Mock<IPaymentNotification>();
            _paymentInteractor = new PaymentInteractor(
                _paymentRepositoryMock.Object,
                _externalProviderMock.Object,
                _paymentNotificationMock.Object
            );
        }

        [Fact]
        public void CreatePayment_ValidRequest_ReturnsSuccessResponse()
        {
            // Arrange
            var requestModel = new CreatePaymentRequestModel
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

            // Act
            var response = _paymentInteractor.CreatePayment(requestModel);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(Status.Created, response.Status);
            Assert.NotEmpty(response.ExternalId);
            Assert.NotEmpty(response.QrData);
            // Add more assertions as needed
        }

    //     [Fact]
    //     public void CreatePayment_InvalidRequest_ReturnsErrorResponse()
    //     {
    //         // Arrange
    //         var requestModel = new CreatePaymentRequestModel
    //         {
    //             // Set up the request model properties with invalid values
    //         };

    //         // Act
    //         var response = _paymentInteractor.CreatePayment(requestModel);

    //         // Assert
    //         Assert.NotNull(response);
    //         Assert.Equal(PaymentStatus.Error, response.Status);
    //         // Add more assertions as needed
    //     }

    //     [Fact]
    //     public void UpdatePayment_ValidRequest_UpdatesPayment()
    //     {
    //         // Arrange
    //         var requestModel = new UpdatePaymentRequestModel
    //         {
    //             // Set up the request model properties
    //         };

    //         // Act
    //         _paymentInteractor.UpdatePayment(requestModel);

    //         // Assert
    //         _paymentRepositoryMock.Verify(r => r.UpdatePayment(It.IsAny<Payment>()), Times.Once);
    //         // Add more assertions as needed
    //     }

    //     [Fact]
    //     public void GetPaymentById_ExistingId_ReturnsPayment()
    //     {
    //         // Arrange
    //         var requestModel = new QueryPaymentByIdRequestModel
    //         {
    //             // Set up the request model properties
    //         };
    //         var payment = new Payment
    //         {
    //             // Set up the payment object
    //         };
    //         _paymentRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).Returns(payment);

    //         // Act
    //         var response = _paymentInteractor.GetPaymentById(requestModel);

    //         // Assert
    //         Assert.NotNull(response);
    //         Assert.Equal(payment.Id, response.Payment.Id);
    //         // Add more assertions as needed
    //     }

    //     [Fact]
    //     public void GetPaymentById_NonExistingId_ReturnsNull()
    //     {
    //         // Arrange
    //         var requestModel = new QueryPaymentByIdRequestModel
    //         {
    //             // Set up the request model properties with a non-existing ID
    //         };
    //         _paymentRepositoryMock.Setup(r => r.GetPaymentById(It.IsAny<Guid>())).Returns((Payment)null);

    //         // Act
    //         var response = _paymentInteractor.GetPaymentById(requestModel);

    //         // Assert
    //         Assert.Null(response);
    //         // Add more assertions as needed
    //     }
    }
}