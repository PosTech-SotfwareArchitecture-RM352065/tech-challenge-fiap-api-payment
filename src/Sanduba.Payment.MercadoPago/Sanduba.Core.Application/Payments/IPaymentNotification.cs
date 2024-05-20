using Sanduba.Core.Application.Payments.RequestModel;
using Sanduba.Core.Application.Payments.ResponseModel.ExternalProvider;
using System.Threading;
using System.Threading.Tasks;

namespace Sanduba.Core.Application.Payments
{
    public interface IPaymentNotification
    {
        public Task UpdatedPayment(PaymentDetailData requestModel, CancellationToken cancellationToken);
    }
}
