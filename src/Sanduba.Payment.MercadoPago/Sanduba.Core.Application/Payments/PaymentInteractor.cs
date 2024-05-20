using Sanduba.Core.Application.Payments.RequestModel;
using Sanduba.Core.Application.Payments.ResponseModel;
using Sanduba.Core.Domain.Payments;
using System;
using System.Threading;
using PaymentStatus = Sanduba.Core.Application.Payments.ResponseModel.ExternalProvider.PaymentStatus;

namespace Sanduba.Core.Application.Payments
{
    public class PaymentInteractor(
        IPaymentRepository paymentRepository,
        IPaymentExternalProvider externalProvider,
        IPaymentNotification paymentNotification
    )
    {
        private readonly IPaymentRepository _paymentRepository = paymentRepository;
        private readonly IPaymentExternalProvider _externalProvider = externalProvider;
        private readonly IPaymentNotification _paymentNotification = paymentNotification;

        public CreatePaymentResponseModel CreatePayment(CreatePaymentRequestModel requestModel)
        {
            var payment = Payment.CreatePayment(
                id: Guid.NewGuid(),
                order: requestModel.Order,
                method: requestModel.Method
            );

            _paymentRepository.SaveAsync(payment, CancellationToken.None).Wait();

            var qrData = _externalProvider.CreateQrCodePayment( payment );
            qrData.Wait();

            payment.SentToExternalPaymentProvider(qrData.Result.ExternalId, qrData.Result.QrCodeData);
            _paymentRepository.UpdateAsync(payment, CancellationToken.None).Wait();

            return new CreatePaymentResponseModel(payment.Id, payment.Status, payment.ExternalId, payment.QrData);
        }

        public void UpdatePayment(UpdatePaymentRequestModel requestModel)
        {
            var payment = _paymentRepository.GetByExternalProviderId(requestModel.Id, CancellationToken.None);
            payment.Wait();

            if (payment.Result == null) return;

            var paymentDetail = _externalProvider.GetPaymentData(payment.Result).Result;

            if (paymentDetail != null && paymentDetail.Status is PaymentStatus.WaitingPayment) return;

            if (paymentDetail.Status == PaymentStatus.Payed) payment.Result.Payed();
            if (paymentDetail.Status == PaymentStatus.Cancelled) payment.Result.Cancelled();

            _paymentRepository.UpdateAsync(payment.Result, CancellationToken.None).Wait();
            _paymentNotification.UpdatedPayment(paymentDetail, CancellationToken.None);
        }

        public QueryPaymentByIdResponseModel GetPaymentById(QueryPaymentByIdRequestModel requestModel)
        {
            var query = _paymentRepository.GetByIdAsync(requestModel.Id, CancellationToken.None);
            query.Wait();

            if(query.Result == null) return null;

            return new QueryPaymentByIdResponseModel(
                        Id: query.Result.Id,
                        Status: query.Result.Status,
                        Order: query.Result.Order,
                        Method: query.Result.Method,
                        Provider: query.Result.Provider,
                        ExternalId: query.Result.ExternalId,
                        QrData: query.Result.QrData);
        }
    }
}
