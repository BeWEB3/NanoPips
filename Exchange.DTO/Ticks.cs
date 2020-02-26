using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange.DTO
{
  
    public class Tick
    {
        public string O { get; set; }
        public string H { get; set; }
        public string L { get; set; }
        public string C { get; set; }
        public string V { get; set; }
        public DateTime T { get; set; }
        public string BV { get; set; }
    }

    public class Ticks
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("result")]
        public List<Tick> Tick { get; set; }
    }
}
