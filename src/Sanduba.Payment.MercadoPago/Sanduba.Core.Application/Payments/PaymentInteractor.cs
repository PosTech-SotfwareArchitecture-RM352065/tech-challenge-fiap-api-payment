using Sanduba.Core.Application.Abstraction.Orders.Events;
using Sanduba.Core.Application.Payments.RequestModel;
using Sanduba.Core.Application.Payments.ResponseModel;
using Sanduba.Core.Domain.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
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
            try
            {
                var payment = Payment.CreatePayment(
                    id: Guid.NewGuid(),
                    order: requestModel.Order,
                    method: requestModel.Method
                );

                _paymentRepository.SaveAsync(payment, CancellationToken.None).Wait();

                var qrData = _externalProvider.CreateQrCodePayment(payment);
                qrData.Wait();

                payment.SentToExternalPaymentProvider(qrData.Result.ExternalId, qrData.Result.QrCodeData);
                _paymentRepository.UpdateAsync(payment, CancellationToken.None).Wait();

                return new CreatePaymentResponseModel(payment.Id, payment.Status, payment.ExternalId, payment.QrData);
            }
            catch (Exception ex)
            {
                return new CreatePaymentResponseModel(Guid.Empty, Status.Error, string.Empty, string.Empty, ex.Message);
            }
        }

        public void SyncExternalStatus(UpdatePaymentRequestModel requestModel)
        {
            var payment = _paymentRepository.GetByExternalProviderId(requestModel.Id, CancellationToken.None).Result;

            if (payment == null) return;

            var paymentDetail = _externalProvider.GetPaymentData(payment).Result;

            if (paymentDetail is null || paymentDetail.Status is PaymentStatus.WaitingPayment) return;

            if (paymentDetail.Status == PaymentStatus.Payed) payment.Payed();
            if (paymentDetail.Status == PaymentStatus.Cancelled) payment.Cancelled();

            _paymentRepository.UpdateAsync(payment, CancellationToken.None).Wait();

            if (paymentDetail.Status == PaymentStatus.Payed)
            {
                var paymentConfirmedEvent = new OrderPaymentConfirmedEvent(payment.Order.Id, payment.Id, payment.Method, payment.Provider);
                _paymentNotification.UpdatedPayment(paymentConfirmedEvent, CancellationToken.None);
            }
            else
            {
                var paymentRejectedEvent = new OrderPaymentRejectedEvent(payment.Order.Id, $"Pagamento em {payment.Method} no {payment.Provider} rejeitado");
                _paymentNotification.UpdatedPayment(paymentRejectedEvent, CancellationToken.None);
            }
        }

        public QueryPaymentByIdResponseModel? GetPaymentById(QueryPaymentByIdRequestModel requestModel)
        {
            var query = _paymentRepository.GetByIdAsync(requestModel.Id, CancellationToken.None);
            query.Wait();

            if (query.Result == null) return null;

            return new QueryPaymentByIdResponseModel(
                        Id: query.Result.Id,
                        Status: query.Result.Status,
                        Order: query.Result.Order,
                        Method: query.Result.Method,
                        Provider: query.Result.Provider,
                        ExternalId: query.Result.ExternalId,
                        QrData: query.Result.QrData);
        }

        public List<QueryPaymentByIdResponseModel> GetAllPayments()
        {
            var payments = _paymentRepository.GetAllAsync(CancellationToken.None).Result;

            if (payments == null) return new List<QueryPaymentByIdResponseModel>();

            return payments.Select(payment =>
                new QueryPaymentByIdResponseModel(
                        Id: payment.Id,
                        Status: payment.Status,
                        Order: payment.Order,
                        Method: payment.Method,
                        Provider: payment.Provider,
                        ExternalId: payment.ExternalId,
                        QrData: payment.QrData)
                ).ToList();
        }
    }
}
