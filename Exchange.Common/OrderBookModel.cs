
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Exchange.Common
{
    public class OrderBookModel
    {
        [JsonProperty("bid")]
        public List<rates> bid { get; set; }
        [JsonProperty("ask")]
        public List<rates> ask { get; set; }

        public class rates {
        [JsonProperty("quantity")]
        public decimal quantity { get; set; }
        [JsonProperty("rate")]
        public decimal rate { get; set; }
        }
    }
}
