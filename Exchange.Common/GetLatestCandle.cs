using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange.Common
{
    public class GetLatestCandle
    {
        public string status { get; set; }
        public Ticker ticker { get; set; }
    }

    public class Day
    {
        public double c { get; set; }
        public double h { get; set; }
        public double l { get; set; }
        public double o { get; set; }
        public double v { get; set; }
        public double vw { get; set; }
    }

    public class LastTrade
    {
        public List<int> c { get; set; }
        public string i { get; set; }
        public double p { get; set; }
        public double s { get; set; }
        public long   t { get; set; }
        public int    x { get; set; }
    }

    public class Min
    {
        public double c { get; set; }
        public double h { get; set; }
        public double l { get; set; }
        public double o { get; set; }
        public double v { get; set; }
        public double vw { get; set; }
    }

    public class PrevDay
    {
        public double c { get; set; }
        public double h { get; set; }
        public double l { get; set; }
        public double o { get; set; }
        public double v { get; set; }
        public double vw { get; set; }
    }

    public class Ticker
    {
        public Day day { get; set; }
        public LastTrade lastTrade { get; set; }
        public Min min { get; set; }
        public PrevDay prevDay { get; set; }
        public string ticker { get; set; }
        public double todaysChange { get; set; }
        public double todaysChangePerc { get; set; }
        public long   updated { get; set; }
    }
}

