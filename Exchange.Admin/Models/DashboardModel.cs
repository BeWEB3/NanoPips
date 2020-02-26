using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoinEXR.Admin.Models
{
    public class DashboardModel
    {
        public int TotalAccounts { get; set; }
        public int TotalSystemAccounts { get; set; }
        public int TotalCurrencies { get; set; }
        public string TotalProfit { get; set; }
        public List<AdminAccounts> adminAccountList = new List<AdminAccounts>();

    }

    public class AdminAccounts
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string accountType { get; set; }
    }
}