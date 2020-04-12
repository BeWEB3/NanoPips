using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange.Common
{
    public class GetSpecificTickerBinance
    {
        [JsonProperty("symbol")]
        public string symbol { get; set; }
        [JsonProperty("bidPrice")]
        public string bidPrice { get; set; }
        [JsonProperty("bidQty")]
        public string bidQty { get; set; }
        [JsonProperty("askPrice")]
        public string askPrice { get; set; }
        [JsonProperty("askQty")]
        public string askQty { get; set; }
    }
}
