using Sanduba.Core.Application.Payments.ResponseModel.ExternalProvider;
using Sanduba.Core.Domain.Payments;
using System.Threading;
using System.Threading.Tasks;

namespace Sanduba.Core.Application.Payments
{
    public interface IPaymentExternalProvider
    {
        public Task<QrCodePaymentData> CreateQrCodePayment(Payment payment);

        public Task<PaymentDetailData> GetPaymentData(Payment payment);
    }
}
