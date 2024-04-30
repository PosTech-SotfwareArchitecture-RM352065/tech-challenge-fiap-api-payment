using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanduba.Core.Domain.Payments
{
    public enum Status
    {
        Created,
        WaitingPayment,
        Payed,
        Expired,
        Cancelled
    }
}
