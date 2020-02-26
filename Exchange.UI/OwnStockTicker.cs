using Exchange.DTO;
using Exchange.UI.App_Start;
using Exchange.UOW;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Ninject;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Script.Serialization;

namespace Exchange.UI
{
    public class OwnStockTicker
    {
        // Singleton instance
        private readonly static Lazy<OwnStockTicker> _instance = new Lazy<OwnStockTicker>(
            () => new OwnStockTicker(GlobalHost.ConnectionManager.GetHubContext<OwnRateHub>().Clients));
        private IHubConnectionContext<dynamic> _clients;
        private readonly object _marketStateLock = new object();
        private static Dictionary<string, Timer> _timers = new Dictionary<string, Timer>();

        public OwnStockTicker(IHubConnectionContext<dynamic> clients)
        {
            this._clients = clients;
        }
        public static OwnStockTicker Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        public void OpenOwnMarket(string market, string clientId)
        {
            lock (_marketStateLock)
            {
                _timers.Add(clientId, new Timer(new TimerCallback(UpdateOwnStockPrices), market + "___" + clientId, 0, 1000));
                _timers.Add(clientId + "-HISTORY", new Timer(new TimerCallback(GetOwnMarketHistory), market + "___" + clientId, 0, 500));
            }
        }
        public void CloseOwnMarket(string clientId)
        {
            lock (_marketStateLock)
            {
                Timer t;
                _timers.TryGetValue(clientId, out t);
                t.Dispose();
                t = null;
                Timer thistory;
                _timers.TryGetValue(clientId + "-HISTORY", out thistory);
                thistory.Dispose();
                thistory = null;

            }
        }
        private void UpdateOwnStockPrices(object state)
        {
            string obj = (string)state;
            string market = obj.Split(new string[] { "___" }, StringSplitOptions.None)[0];
            string clientId = obj.Split(new string[] { "___" }, StringSplitOptions.None)[1];
            var data = new { Summary = new JavaScriptSerializer().Serialize(NinjectWebCommon.GlobalKernal.Get<IUnitOfWork>().MarketRates.GetOwnMarketSummaries().Select(m => new { Bid = m.Bid, Ask = m.Ask, High = m.High.ToString("0.0000"), Low = m.Low.ToString("0.0000"), Volume = m.Volume.ToString("0.0"), MarketName = m.Symbol }).ToList()) };

            _clients.Client(clientId).updateOwnMarket(data);
        }
        private void GetOwnMarketHistory(object state)
        {
            string obj = (string)state;
            string market = obj.Split(new string[] { "___" }, StringSplitOptions.None)[0];
            string clientId = obj.Split(new string[] { "___" }, StringSplitOptions.None)[1];
            var history = NinjectWebCommon.GlobalKernal.Get<IUnitOfWork>().MarketRates.GetMarketHistory(market);
            var result = NinjectWebCommon.GlobalKernal.Get<IUnitOfWork>().MarketRates.GetOrderSummaries(market);
            var buyOrders = result.Where(x => x.Direction == "BUY").OrderByDescending(x => x.Rate).GroupBy(x => x.Rate).Select(s => {
                var item = s.First();
                return new Trade
                {
                    Rate = item.Rate,
                    Amount = s.Sum(e => e.Amount),
                    Remaining = item.Remaining,
                    Status = item.Status
                };
            }).ToList();
            var sellOrders = result.Where(x => x.Direction == "SELL").OrderByDescending(x => x.Rate).GroupBy(x => x.Rate).Select(s => {
                var item = s.First();
                return new Trade
                {
                    Rate = item.Rate,
                    Amount = s.Sum(e => e.Amount),
                    Remaining = item.Remaining,
                    Status = item.Status
                };
            }).ToList();

            var data = new { history = new JavaScriptSerializer().Serialize(history), buy = new JavaScriptSerializer().Serialize(buyOrders), sell = new JavaScriptSerializer().Serialize(sellOrders) };
            
            _clients.Client(clientId).updateOwnHistory(data);
        }
    }
}