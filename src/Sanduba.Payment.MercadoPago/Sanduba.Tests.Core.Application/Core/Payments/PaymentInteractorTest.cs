using System;
using Xunit;
using Moq;
using Sanduba.Core.Application.Payments;
using Sanduba.Core.Application.Payments.RequestModel;
using Sanduba.Core.Application.Payments.ResponseModel.ExternalProvider;
using Sanduba.Core.Domain.Payments;
using Sanduba.Core.Domain.Orders;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace Sanduba.Test.Unit.Core.Payments
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
            var requestModel = PaymentFixture.ValidPaymentCreationRequestModel();

            _paymentRepositoryMock
                .Setup(repo => repo.SaveAsync(It.IsAny<Payment>(), CancellationToken.None)).Returns(Task.CompletedTask);
            _externalProviderMock
                .Setup(provider => provider.CreateQrCodePayment(It.IsAny<Payment>())).ReturnsAsync(new QrCodePaymentData("ExternalId", "QrCodeData"));
            _paymentRepositoryMock
                .Setup(repo => repo.UpdateAsync(It.IsAny<Payment>(), CancellationToken.None)).Returns(Task.CompletedTask);

            // Act
            var response = _paymentInteractor.CreatePayment(requestModel);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(Status.WaitingPayment, response.Status);
            Assert.Equal("ExternalId", response.ExternalId);
            Assert.Equal("QrCodeData", response.QrData);
            Assert.Null(response.Message);
        }

        [Fact]
        public void CreatePayment_InvalidRequest_ReturnsErrorResponse()
        {
            // Arrange
            var requestModel = PaymentFixture.InvalidPaymentCreationRequestModel();

            _paymentRepositoryMock
                .Setup(repo => repo.SaveAsync(It.IsAny<Payment>(), CancellationToken.None)).Returns(Task.CompletedTask);
            _externalProviderMock
                .Setup(provider => provider.CreateQrCodePayment(It.IsAny<Payment>())).Throws(new Exception("Error during payment creation"));
            _paymentRepositoryMock
                .Setup(repo => repo.UpdateAsync(It.IsAny<Payment>(), CancellationToken.None)).Returns(Task.CompletedTask);

            // Act
            var response = _paymentInteractor.CreatePayment(requestModel);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(Status.Error, response.Status);
            Assert.Empty(response.ExternalId);
            Assert.Empty(response.QrData);
            Assert.Equal("Pedido deve conter os itens!", response.Message);
        }

        [Fact]
        public void UpdatePayment_ValidRequest_UpdatesPayment()
        {
            // Arrange
            var requestModel = new UpdatePaymentRequestModel(long.MaxValue);
            var payment = PaymentFixture.PaymentSentToProvider(Guid.NewGuid());

            var paymentDetail = new PaymentDetailData(PaymentStatus.Payed, "ExternalPaymentId", DateTimeOffset.Now);

            _paymentRepositoryMock
                .Setup(repo => repo.GetByExternalProviderId(requestModel.Id, CancellationToken.None)).ReturnsAsync(payment);
            _externalProviderMock
                .Setup(provider => provider.GetPaymentData(payment)).ReturnsAsync(paymentDetail);
            _paymentRepositoryMock
                .Setup(repo => repo.UpdateAsync(It.IsAny<Payment>(), It.IsAny<CancellationToken>()));

            // Act
            _paymentInteractor.SyncExternalStatus(requestModel);

            // Assert
            Assert.Equal(Status.Payed, payment.Status);
            _paymentRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Payment>(), It.IsAny<CancellationToken>()), Times.Once);
            _paymentNotificationMock.Verify(broker => broker.UpdatedPayment(paymentDetail, CancellationToken.None), Times.Once);
        }

        [Fact]
        public void UpdatePayment_UnchangedRequest_Ignores()
        {
            // Arrange
            var requestModel = new UpdatePaymentRequestModel(long.MaxValue);
            var payment = PaymentFixture.PaymentSentToProvider(Guid.NewGuid());

            var paymentDetail = new PaymentDetailData(PaymentStatus.WaitingPayment, null, null);

            _paymentRepositoryMock
                .Setup(repo => repo.GetByExternalProviderId(requestModel.Id, CancellationToken.None)).ReturnsAsync(payment);
            _externalProviderMock
                .Setup(provider => provider.GetPaymentData(payment)).ReturnsAsync(paymentDetail);
            _paymentRepositoryMock
                .Setup(repo => repo.UpdateAsync(It.IsAny<Payment>(), It.IsAny<CancellationToken>()));

            // Act
            _paymentInteractor.SyncExternalStatus(requestModel);

            // Assert
            Assert.Equal(Status.WaitingPayment, payment.Status);
            _paymentRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Payment>(), It.IsAny<CancellationToken>()), Times.Never);
            _paymentNotificationMock.Verify(broker => broker.UpdatedPayment(paymentDetail, CancellationToken.None), Times.Never);
        }

        [Fact]
        public void UpdatePayment_CancelledRequest_CancelsPayment()
        {
            // Arrange
            var requestModel = new UpdatePaymentRequestModel(long.MaxValue);
            var payment = PaymentFixture.PaymentSentToProvider(Guid.NewGuid());

            var paymentDetail = new PaymentDetailData(PaymentStatus.Cancelled, null, null);

            _paymentRepositoryMock
                .Setup(repo => repo.GetByExternalProviderId(requestModel.Id, CancellationToken.None)).ReturnsAsync(payment);
            _externalProviderMock
                .Setup(provider => provider.GetPaymentData(payment)).ReturnsAsync(paymentDetail);
            _paymentRepositoryMock
                .Setup(repo => repo.UpdateAsync(It.IsAny<Payment>(), It.IsAny<CancellationToken>()));

            // Act
            _paymentInteractor.SyncExternalStatus(requestModel);

            // Assert
            Assert.Equal(Status.Cancelled, payment.Status);
            _paymentRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Payment>(), It.IsAny<CancellationToken>()), Times.Once);
            _paymentNotificationMock.Verify(broker => broker.UpdatedPayment(paymentDetail, CancellationToken.None), Times.Once);
        }

        [Fact]
        public void GetPaymentById_ExistingId_ReturnsPayment()
        {
            // Arrange
            var paymentId = Guid.NewGuid();
            var requestModel = new QueryPaymentByIdRequestModel(paymentId);
            var payment = PaymentFixture.PaymentCreated(paymentId);

            _paymentRepositoryMock
                .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(payment);

            // Act
            var response = _paymentInteractor.GetPaymentById(requestModel);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(payment.Id, response.Id);
            Assert.Equal(payment.ExternalId, response.ExternalId);
            Assert.Equal(payment.Order, response.Order);
            Assert.Equal(payment.QrData, response.QrData);
            Assert.Equal(payment.Status, response.Status);
            Assert.Equal(payment.Method, response.Method);
            Assert.Equal(payment.Provider, response.Provider);
            // Add more assertions as needed
        }

        [Fact]
        public void GetPaymentById_NonExistingId_ReturnsNull()
        {
            // Arrange
            var requestModel = new QueryPaymentByIdRequestModel(Guid.NewGuid());
            _paymentRepositoryMock
                .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync((Payment)null);

            // Act
            var response = _paymentInteractor.GetPaymentById(requestModel);

            // Assert
            Assert.Null(response);
            // Add more assertions as needed
        }
    }
}