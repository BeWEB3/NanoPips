using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Exchange.DTO
{
    public class MarketSummariesV1
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("result")]
        public List<ResultData> ResultData { get; set; }
    }

    public class ResultData
    {
        [JsonProperty("MarketName")]
        public string MarketName { get; set; }

        [JsonProperty("High")]
        public decimal High { get; set; }

        [JsonProperty("Low")]
        public decimal Low { get; set; }

        [JsonProperty("Volume")]
        public decimal Volume { get; set; }

        [JsonProperty("Last")]
        public decimal Last { get; set; }

        [JsonProperty("BaseVolume")]
        public decimal BaseVolume { get; set; }

        [JsonProperty("TimeStamp")]
        public DateTime TimeStamp { get; set; }

        [JsonProperty("Bid")]
        public decimal Bid { get; set; }

        [JsonProperty("Ask")]
        public decimal Ask { get; set; }

        [JsonProperty("OpenBuyOrders")]
        public long OpenBuyOrders { get; set; }

        [JsonProperty("OpenSellOrders")]
        public long OpenSellOrders { get; set; }

        [JsonProperty("PrevDay")]
        public decimal PrevDay { get; set; }

        [JsonProperty("Created")]
        public DateTime Created { get; set; }
    }

}
