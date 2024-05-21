using Sanduba.Core.Domain.Payments;
using System;

namespace Sanduba.Core.Application.Payments.ResponseModel
{
    public record CreatePaymentResponseModel(Guid Id, Status Status, string ExternalId, string QrData, string? Message = null);
}
