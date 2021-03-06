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
    
    public partial class Payment
    {
        public long PaymentId { get; set; }
        public string Currency { get; set; }
        public Nullable<System.DateTime> PaymentDate { get; set; }
        public string ToWalletAddress { get; set; }
        public string PaymentType { get; set; }
        public string StatusMessage { get; set; }
        public string TransectionId { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> AmountSent { get; set; }
        public Nullable<decimal> NetworkFee { get; set; }
        public Nullable<decimal> Fee { get; set; }
        public Nullable<int> Confirms { get; set; }
        public Nullable<long> Account_Id { get; set; }
        public Nullable<int> PaymentStatus_Id { get; set; }
        public string FiatCurrency { get; set; }
        public Nullable<decimal> FiatAmount { get; set; }
        public string BankAccountTitle { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankName { get; set; }
        public string Source { get; set; }
        public string Reason { get; set; }
        public string BankAddress { get; set; }
        public string IBAN { get; set; }
        public string SwiftCode { get; set; }
        public string BankCity { get; set; }
        public string BankCountry { get; set; }
        public Nullable<double> Spread { get; set; }
        public string BankAccountType { get; set; }
        public Nullable<double> ActualAmount { get; set; }
    
        public virtual Account Account { get; set; }
    }
}
