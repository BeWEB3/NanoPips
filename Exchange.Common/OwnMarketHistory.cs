using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange.Common
{
    public class OwnMarketHistory
    {
        public string CompletedAt { get; set; }
        public double? Rate { get; set; }
        public double? Amount { get; set; }
        public string Type { get; set; }
    }
}
