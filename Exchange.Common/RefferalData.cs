using Exchange.DTO;
using System;
using System.Collections.Generic;

namespace Exchange.Common
{
    public class RefferalData
    {
        public List<userList> refferalUsers { get; set; }
        public List<userList> refferalTier2Users { get; set; }
        public List<rank> rankList { get; set; }
        public string     referralCode { get; set; }
        public long       refferalCount { get; set; }
        public decimal    earning { get; set; }
        public DateTime   dateTime { get; set; }
        public List<referralSubtractList> referralSubtract { get; set; }
    }
    public class userList
    {
        public string  email { get; set; }
        public decimal amount { get; set; }
    }

    public class referralSubtractList
    {
        public DateTime dateTime { get; set; }
        public string   reason { get; set; }
        public decimal  amount { get; set; }
    }

    public class rank
    {
        public rank(string email, int rankIndex, decimal amount) {
            this.email    = email;
            this.amount   = amount;
            this.rankIndex = rankIndex;
        }

        public string email { get; set; }
        public int rankIndex { get; set; }
        public decimal amount { get; set; }
    }
}
