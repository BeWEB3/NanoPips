using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange.Common
{
    public enum LaunchPadApplicationStatus
    {
        PENDING = 1,
        REVIEWING = 2,
        CANCELED = 3,
        ACCEPTED = 4,
        REJECTED = 5,
        REAPPLY = 6
    }
}
