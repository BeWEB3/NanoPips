using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinanceExchange.API.Models.WebSocket;

namespace TestConsole.cs
{
    public class CandleData
    {
        public string e { get; set; }
        public long E { get; set; }
        public string s { get; set; }
        public K K { get; set; }

        public static implicit operator CandleData(BinanceKlineData v)
        {
            throw new NotImplementedException();
        }
    }

        public class K
        {
            public long t { get; set; }
            public long T { get; set; }
            public string s { get; set; }
            public string i { get; set; }
            public int f { get; set; }
            public int L { get; set; }
            public double o { get; set; }
            public double c { get; set; }
            public double h { get; set; }
            public double l { get; set; }
            public double v { get; set; }
            public int n { get; set; }
            public bool x { get; set; }
            public double q { get; set; }
            public double V { get; set; }
            public double Q { get; set; }
        }
}
