using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Exchange.UI.Models
{
    public class AdminDashboard
    {
        public decimal? Revenue { get; set; }
        public decimal?[] Deposits { get; set; }
        public decimal?[] Withdrawls { get; set; }
        public decimal?[] Buy { get; set; }
        public decimal?[] Sell { get; set; }
    }
   
}