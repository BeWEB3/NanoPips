using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange.DTO
{
    public partial class RateHistory
    {
        [JsonProperty("Response")]
        public string Response { get; set; }

        [JsonProperty("Type")]
        public long Type { get; set; }

        [JsonProperty("Aggregated")]
        public bool Aggregated { get; set; }

        [JsonProperty("Data")]
        public Datum[] Data { get; set; }

        [JsonProperty("TimeTo")]
        public long TimeTo { get; set; }

        [JsonProperty("TimeFrom")]
        public long TimeFrom { get; set; }

        [JsonProperty("FirstValueInArray")]
        public bool FirstValueInArray { get; set; }

        [JsonProperty("ConversionType")]
        public ConversionType ConversionType { get; set; }
    }

    public partial class Datum
    {
        [JsonProperty("time")]
        public long Time { get; set; }

        [JsonProperty("close")]
        public double Close { get; set; }

        [JsonProperty("high")]
        public double High { get; set; }

        [JsonProperty("low")]
        public double Low { get; set; }

        [JsonProperty("open")]
        public double Open { get; set; }

        [JsonProperty("volumefrom")]
        public double Volumefrom { get; set; }

        [JsonProperty("volumeto")]
        public double Volumeto { get; set; }
    }

    public partial class ConversionType
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("conversionSymbol")]
        public string ConversionSymbol { get; set; }
    }
}
