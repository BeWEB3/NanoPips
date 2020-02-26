using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange.DTO
{
   
    public class MHistory
    {
        public int Id { get; set; }
        public string TimeStamp { get; set; }
        public string Quantity { get; set; }
        public decimal Price { get; set; }
        public string PriceString => Price.ToString("N", new CultureInfo("en-US"));
        public string Total { get; set; }
        public string FillType { get; set; }
        public string OrderType { get; set; }
    }

    public class MarketHistory
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("result")]
        public List<MHistory> History { get; set; }
    }
}
