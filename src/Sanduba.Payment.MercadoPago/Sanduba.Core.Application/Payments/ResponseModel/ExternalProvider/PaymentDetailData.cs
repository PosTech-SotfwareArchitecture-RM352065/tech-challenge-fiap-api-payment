using System;

namespace Sanduba.Core.Application.Payments.ResponseModel.ExternalProvider
{
    public record PaymentDetailData (PaymentStatus Status, string? PaymentId, DateTimeOffset? PayedAt);
}
