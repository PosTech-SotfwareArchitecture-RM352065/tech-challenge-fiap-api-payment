using Microsoft.Extensions.Logging;
using Moq;
using Sanduba.Cloud.Function.MercadoPago;
using Sanduba.Core.Application.Payments;
using System;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Sanduba.Core.Application.Payments.ResponseModel.ExternalProvider;
using Sanduba.Core.Domain.Payments;
using System.Threading;
using Sanduba.Core.Application.Payments.ResponseModel;

namespace Sanduba.Test.Unit.Cloud.Function
{
    public class PaymentCreationTest
    {
        private readonly Mock<ILogger<PaymentCreation>> _logger = new();
        private readonly Mock<IPaymentRepository> _paymentRepositoryMock = new();
        private readonly Mock<IPaymentExternalProvider> _paymentExternalProviderMock = new();
        private readonly Mock<IPaymentNotification> _paymentNotificationMock = new();
        private readonly PaymentCreation _paymentCreation;

        public PaymentCreationTest()
        {
            _paymentCreation = new PaymentCreation(
                _logger.Object,
                _paymentRepositoryMock.Object,
                _paymentExternalProviderMock.Object,
                _paymentNotificationMock.Object
            );
        }

        [Fact]
        public void GivenValidOrder_WhenCreatePayment_ThenReturnOkObjectResult()
        {
            // Arrange
            var request = PaymentCreationFixture.ValidPaymentCreationHttpRequest();
            _paymentRepositoryMock
                .Setup(repo => repo.SaveAsync(It.IsAny<Payment>(), CancellationToken.None)).Returns(Task.CompletedTask);
            _paymentExternalProviderMock
                .Setup(provider => provider.CreateQrCodePayment(It.IsAny<Payment>())).ReturnsAsync(new QrCodePaymentData("ExternalId", "QrCodeData"));
            _paymentRepositoryMock
                .Setup(repo => repo.UpdateAsync(It.IsAny<Payment>(), CancellationToken.None)).Returns(Task.CompletedTask);

            // Act
            var result = _paymentCreation.Create(request);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<CreatePaymentResponseModel>((result as OkObjectResult).Value);
        }

        [Fact]
        public void GivenInvalidOrder_WhenCreatePayment_ThenReturnError()
        {
            // Arrange
            var request = PaymentCreationFixture.InvalidPaymentCreationHttpRequest();

            // Act
            var result = _paymentCreation.Create(request);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<CreatePaymentResponseModel>((result as BadRequestObjectResult).Value);
        }
    }
}
