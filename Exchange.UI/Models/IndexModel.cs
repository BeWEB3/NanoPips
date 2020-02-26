using Exchange.Common;
using Exchange.DTO;
using System;
using System.Collections.Generic;

namespace Exchange.UI.Models
{
    public class IndexModel
    {
        public bool AcceptTermConditions { get; set; }
        public List<Pair> GetPairs { get; set; }
        public List<Trade> GetTradeHistory { get; set; }
        public string MarketName { get; set; }
        public object GetCandles { get; set; }
        public GetOrderBookBinance GetOrderBook { get; set; }
        public string TimeFrameValue { get; set; }
        public string referral { get; set; }
        public decimal timeOffset { get; set; }
        public UserRole UserRoles { get; set; }
        public bool IsLogin { get; set; }
    }
}