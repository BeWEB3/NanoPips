using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange.Common
{
    public class GetPolygonCandles
    {
        public string ticker { get; set; }
        public string status { get; set; }
        public int queryCount { get; set; }
        public int resultsCount { get; set; }
        public bool adjusted { get; set; }
        public List<Ticker> results { get; set; }

        public class Ticker
        {
            public decimal v { get; set; }
            public decimal o { get; set; }
            public decimal c { get; set; }
            public decimal h { get; set; }
            public decimal l { get; set; }
            public long   t { get; set; }
            public int    n { get; set; }
        }


    }
}
