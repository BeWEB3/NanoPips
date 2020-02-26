using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exchange.DTO;

namespace Exchange.UI.Models
{
    public class ProfileModel
    {
        public ChangePassword ChangePassword { get; set; }
        public List<Payment> DepositHistory { get; set; }
        public List<Payment> WithdrawHistory { get; set; }
        public List<Trade> TradeHistory { get; set; }
        public List<Currency> CurrencyList { get; set; }
        public KYC KYC { get; set; }
        public Payment Payment { get; set; }
        public securityModel securityAnswer { get; set; }
        public Account GetAccount { get; set; }
    }
}
