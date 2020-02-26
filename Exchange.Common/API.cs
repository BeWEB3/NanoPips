using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange.Common
{
    public class Response<T>
    {
        public bool Successful { get; set; }
        public string Exception { get; set; }
        public T Data { get; set; }
    }
}
