using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange.Common
{
    public static class ExtentionMethods
    {
        public static string ToLocalCommasFormat(this decimal value)
        {
            return value.ToString("N", new CultureInfo("en-US"));
        }
    }
}
