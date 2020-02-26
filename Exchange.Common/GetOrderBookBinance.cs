using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange.Common
{
    public class GetOrderBookBinance
    {
        public long lastUpdateId { get; set; }
        public List<List<string>> bids { get; set; }
        public List<List<string>> asks { get; set; }
    }
}
