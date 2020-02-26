using Newtonsoft.Json;

namespace Exchange.Common
{
    public class TradesOnChartResponse
    {
        [JsonProperty("id")]
        public long id { get; set; }

        [JsonProperty("price")]
        public decimal price { get; set; }

        [JsonProperty("time")]
        public string time { get; set; }

        [JsonProperty("date_time")]
        public string dateTime { get; set; }

        [JsonProperty("splitPrice")]
        public decimal splitPrice { get; set; }
    }
}
