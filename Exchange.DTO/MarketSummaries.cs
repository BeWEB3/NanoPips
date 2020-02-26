using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange.DTO
{


    public class Markets
    {
        public string MarketCurrency { get; set; }
        public string BaseCurrency { get; set; }
        public string MarketCurrencyLong { get; set; }
        public string BaseCurrencyLong { get; set; }
        public decimal MinTradeSize { get; set; }
        public string MarketName { get; set; }
        public bool IsActive { get; set; }
        public DateTime Created { get; set; }
        public string Notice { get; set; }
        public bool? IsSponsored { get; set; }
        public string LogoUrl { get; set; }
    }

    public class Summary
    {
        public string MarketName { get; set; }
        public decimal? High { get; set; }
        public decimal? Low { get; set; }
        public decimal? Volume { get; set; }
        public decimal? Last { get; set; }
        public decimal? BaseVolume { get; set; }
        public DateTime TimeStamp { get; set; }
        public decimal? Bid { get; set; }
        public decimal? Ask { get; set; }
        public decimal? ActualBid { get; set; }
        public decimal? ActualAsk { get; set; }
        public int? OpenBuyOrders { get; set; }
        public int? OpenSellOrders { get; set; }
        public decimal? PrevDay { get; set; }
        public DateTime Created { get; set; }
    }

    public class MarketSummary
    {
        public Markets Market { get; set; }
        public Summary Summary { get; set; }
        public bool IsVerified { get; set; }
    }

    public class MarketSummaries
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("result")]
        public List<MarketSummary> MarketSummary { get; set; }
    }
}
