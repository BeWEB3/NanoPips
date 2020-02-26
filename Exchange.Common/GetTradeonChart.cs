using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange.Common
{
    public class GetTradeonChart
    {
        [JsonProperty("id")]
        public long id { get; set; }

        [JsonProperty("count")]
        public long count { get; set; }

        [JsonProperty("type_id")]
        public int type_id { get; set; }

        [JsonProperty("volume")]
        public string volume { get; set; }

        [JsonProperty("price")]
        public string price { get; set; }

        [JsonProperty("summ")]
        public string summ { get; set; }

        [JsonProperty("profit")]
        public string profit { get; set; }

        [JsonProperty("time")]
        public string time { get; set; }

        [JsonProperty("date_time")]
        public string date_time { get; set; }

        [JsonProperty("splitPrice")]
        public decimal splitPrice { get; set; }

        [JsonProperty("security_id")]
        public long security_id { get; set; }

        [JsonProperty("qb")]
        public string qb { get; set; }
       
    }
}
