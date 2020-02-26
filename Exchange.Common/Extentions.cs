using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange.Common
{
    public static class Extentions
    {
        public static DateTime ToDateTime(this string timeStamp)
        {
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Convert.ToInt64(timeStamp)).ToLocalTime();
            return dt;
        }
    }
}
