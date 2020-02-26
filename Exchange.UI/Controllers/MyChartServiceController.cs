using BittrexV3.Lib.Models;
using Exchange.Common;
using Exchange.UOW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Exchange.UI.Controllers
{
    public class MyChartServiceController : Controller
    {
        private IUnitOfWork _uow;
        public MyChartServiceController(IUnitOfWork uow)
        {
            _uow = uow;
        }
        // GET: ChartService
        public string config()
        {
            var data = @"{
                          ""supports_search"":true,
                          ""supports_group_request"":false,
                          ""supports_marks"":false,
                          ""supports_timescale_marks"":false,
                          ""supports_time"":true,
                          ""supported_resolutions"":[""1"",""5"",""30"",""60"",""90"",""D"",""2D"",""15D"",""1M""]
                         }";
            return data;
        }
        public string symbols(string symbol)
        {
            string data = @"{""name"":""" + symbol + @""",
                          ""exchange-traded"":""iBitt"",
                          ""exchange-listed"":""iBitt"",
                           ""timezone"":""America/New_York"",
                           ""minmov"":1,
                           ""minmov2"":0,
                           ""pointvalue"":1,
                           ""session"":""24x7"",
                          ""has_intraday"":true,
                          ""has_no_volume"":false,
                           ""description"":"""",
                          ""type"":""" + symbol + @""",""supported_resolutions"":[""1"",""5"",""30"",""60"",""90"",""D"",""2D"",""15D"",""1M""],
                          ""pricescale"":10000000,""ticker"":""" + symbol + @"""}";
            return data;
        }
        public string history(string symbol, string from, string to, string resolution)
        {
            SessionItems.Add(SessionKey.LOAD_CHART, true);

            List<GetCandlesBySymbol> res = new List<GetCandlesBySymbol>();
            if (resolution == "1")
            {
                res = _uow.MarketRates.GetOwnTicks(symbol, 1);

            }
            else if (resolution == "5")
            {
                res = _uow.MarketRates.GetOwnTicks(symbol, 5);

            }
            else if (resolution == "30")
            {
                res = _uow.MarketRates.GetOwnTicks(symbol, 30);

            }
            else if (resolution == "60" || resolution == "90")
            {
                res = _uow.MarketRates.GetOwnTicks(symbol, 60);

            }
            else
            {
                res = _uow.MarketRates.GetOwnTicks(symbol, 1440);
            }

            ////////////
            //// GET SPREAD
            ///////////////

            decimal spread = _uow.MarketRates.GetBuySpread(symbol.Split('-')[1]);

            string t = "[";
            string o = "[";
            string h = "[";
            string l = "[";
            string c = "[";
            string v = "[";

            for (int i = 0; i < res.Count(); i++)
            {
                t += (Int32)(res[i].StartsAt.ToUniversalTime().Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                var open = Convert.ToDecimal(res[i].Open).ToString();
                var high = Convert.ToDecimal(res[i].High).ToString();
                var low = Convert.ToDecimal(res[i].Low).ToString();
                var close = Convert.ToDecimal(res[i].Close).ToString();
                
                o += open; h += high; l += low; c += close; v += 10;

                if (i != (res.Count() - 1)) 
                {
                    t += ","; o += ","; h += ","; l += ","; c += ","; v += ",";
                }
                else
                {
                    t += "]"; o += "]"; h += "]"; l += "]"; c += "]"; v += "]";
                }

            }
            string data = @"{""t"":" + t + @",
                             ""o"":" + o + @",
                             ""h"":" + h + @",
                             ""l"":" + l + @",
                             ""c"":" + c + @",
                             ""v"":" + v + @",""s"":""ok""}";
            Session[symbol + "_DATA"] = data;
            return data;
        }
        public JsonResult time()
        {
            return Json((TimeZoneInfo.ConvertTimeToUtc(DateTime.Now) -
           new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)).TotalSeconds, JsonRequestBehavior.AllowGet);
        }
    }
}