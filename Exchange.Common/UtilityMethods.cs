using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange.Common
{
    public class UtilityMethods
    {
        public static String convertSymbol(String symbol) {
            try
            {
                String[] sSymbol = symbol.Split('-');
                return sSymbol[0] + sSymbol[1];
            }
            catch (Exception ex) {
                return "";
            }
        }
    }
}
