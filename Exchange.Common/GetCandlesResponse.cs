using Newtonsoft.Json;
using System;

namespace Exchange.Common
{
    public class GetCandlesResponse
    {
        [JsonProperty("startsAt")]
        public DateTime StartsAt { get; set; }

        [JsonProperty("open")]
        public decimal? Open { get; set; }

        [JsonProperty("high")]
        public decimal? High { get; set; }

        [JsonProperty("low")]
        public decimal? Low { get; set; }

        [JsonProperty("close")]
        public decimal? Close { get; set; }

        [JsonProperty("volume")]
        public decimal? Volume { get; set; }

        [JsonProperty("baseVolume")]
        public decimal? BaseVolume { get; set; }
    }
}
