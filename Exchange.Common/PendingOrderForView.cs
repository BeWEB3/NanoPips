using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange.Common
{
    public class PendingOrderForView
    {
        public long    TradeId { get; set; }
        public string  TradeType { get; set; }
        public string  TradeDate { get; set; }
        public string  Currency { get; set; }
        public string  Symbol { get; set; }
        public decimal Amount { get; set; }
        public decimal Rate { get; set; }
        public decimal? PnL { get; set; }
        public decimal? ExitPrice { get; set; }
        public string  Status { get; set; }
        public long    Account_Id { get; set; }
        public decimal Value { get; set; }
        public string  Direction { get; set; }
        public long    TradeTypeValue { get; set; }
        public long    Ticker { get; set; }
        public long    expiryTime { get; set; }
        public bool    isTradeOrder { get; set; }
        public Nullable<decimal> UpPrice { get; set; }
        public Nullable<decimal> DownPrice { get; set; }
        public Nullable<bool>    ST_Enable { get; set; }
    }
}
