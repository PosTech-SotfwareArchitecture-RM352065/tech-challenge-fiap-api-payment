using Microsoft.Extensions.Logging;
using Moq;
using Sanduba.Cloud.Function.MercadoPago;
using Sanduba.Core.Application.Payments;
using System;
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
                .Setup(provider => provider.CreateQrCodePayment(It.IsAny<Payment>())).ReturnsAsync(PaymentCreationFixture.ValidPaymentCreationResponse());
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

        [Fact]
        public void GivenEmptyOrder_WhenCreatePayment_ThenReturnError()
        {
            // Arrange
            var request = PaymentCreationFixture.EmptyHttpRequest();

            // Act
            var result = _paymentCreation.Create(request);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void GivenBadFormatOrder_WhenCreatePayment_ThenReturnError()
        {
            // Arrange
            var request = PaymentCreationFixture.BadFormatOrderHttpRequest();

            // Act
            var result = _paymentCreation.Create(request);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void GivenAnyOrder_WhenUnhandledExceptionOccurs_ThenReturnError()
        {
            // Arrange
            var request = PaymentCreationFixture.ValidPaymentCreationHttpRequest();
            _paymentRepositoryMock
                .Setup(repo => repo.SaveAsync(It.IsAny<Payment>(), CancellationToken.None)).Throws(new Exception("Unhandeled exception"));


            // Act
            var result = _paymentCreation.Create(request);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void GivenValidId_WhenQueryPayment_ThenReturnOkObjectResult()
        {
            // Arrange
            var request = PaymentCreationFixture.ValidPaymentQueryHttpRequest();
            _paymentRepositoryMock
                .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(PaymentCreationFixture.ValidPaymentQueryResponse());

            // Act
            var result = _paymentCreation.Get(request);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<QueryPaymentByIdResponseModel>((result as OkObjectResult).Value);
            _paymentRepositoryMock.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public void GivenInvalidId_WhenQueryPayment_ThenReturnBadRequestObjectResult()
        {
            // Arrange
            var request = PaymentCreationFixture.InvalidPaymentQueryHttpRequest();
            _paymentRepositoryMock
                .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(PaymentCreationFixture.ValidPaymentQueryResponse());

            // Act
            var result = _paymentCreation.Get(request);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            _paymentRepositoryMock.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public void GivenEmptyId_WhenQueryPayment_ThenReturnBadRequestObjectResult()
        {
            // Arrange
            var request = PaymentCreationFixture.EmptyPaymentQueryHttpRequest();
            _paymentRepositoryMock
                .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(PaymentCreationFixture.ValidPaymentQueryResponse());

            // Act
            var result = _paymentCreation.Get(request);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            _paymentRepositoryMock.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public void GivenNonExistingId_WhenQueryPayment_ThenReturnNotFoundResult()
        {
            // Arrange
            var request = PaymentCreationFixture.ValidPaymentQueryHttpRequest();
            _paymentRepositoryMock
                .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>(), CancellationToken.None));

            // Act
            var result = _paymentCreation.Get(request);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            _paymentRepositoryMock.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
