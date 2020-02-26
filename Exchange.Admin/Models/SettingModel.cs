using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Exchange.UI.Models
{
    public class SettingModel
    {
        [Required]
        public string Network { get; set; }
        [Required]
        public decimal BuyPrice { get; set; }
        [Required]
        public decimal SellPrice { get; set; }
        [Required]
        public decimal WithdrawLimit { get; set; }
        [Required]
        public decimal MinimumLimit { get; set; }
        [Required]
        public string ExchangeStatus { get; set; }
        [Required]
        public string CardFee { get; set; }
        [Required]
        public decimal TradeFee { get; set; }

    }
}