using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange.Common
{
    public enum PaymentStatusType
    {
        REJECTED = -1,
        WAITING_CONFIRMATION = 0,
        CREATED  = 1,
        COMPLETED = 2,
    }
}
