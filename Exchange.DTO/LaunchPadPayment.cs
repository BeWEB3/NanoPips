//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Exchange.DTO
{
    using System;
    using System.Collections.Generic;
    
    public partial class LaunchPadPayment
    {
        public long Id { get; set; }
        public Nullable<long> LaunchPadCurrency_id { get; set; }
        public Nullable<long> LaunchPadWallet_id { get; set; }
        public string BuyCurrencyName { get; set; }
        public string BuyWithCurrencyName { get; set; }
        public Nullable<decimal> GetAmount { get; set; }
        public Nullable<decimal> GiveAmount { get; set; }
        public Nullable<decimal> GiveAmountInUSD { get; set; }
        public Nullable<System.DateTime> CreatedDateTime { get; set; }
        public Nullable<bool> IsSupply { get; set; }
        public Nullable<decimal> SupplyAmount { get; set; }
    
        public virtual LaunchPadCurrency LaunchPadCurrency { get; set; }
        public virtual LaunchPadWallet LaunchPadWallet { get; set; }
    }
}
