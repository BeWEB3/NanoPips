using Newtonsoft.Json;

namespace Exchange.Common
{
    public class GetSpecificTicker
    {
        [JsonProperty("symbol")]
        public string symbol { get; set; }

        [JsonProperty("lastTradeRate")]
        public decimal lastTradeRate { get; set; }

        [JsonProperty("bidRate")]
        public decimal bidRate { get; set; }

        [JsonProperty("askRate")]
        public decimal askRate { get; set; }
    }
}
