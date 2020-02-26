using BittrexV3.Lib.Models;
using System.Collections.Generic;

namespace Exchange.Common
{
    public class OwnChart
    {
        public static List<GetCandlesBySymbol> data = new List<GetCandlesBySymbol>();
        public static int _resolution;
        public static string _market = "";
        public static int _total;
    }
}
