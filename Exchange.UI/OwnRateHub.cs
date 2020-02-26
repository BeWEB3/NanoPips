using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace Exchange.UI
{
    public class OwnRateHub : Hub
    {
        private readonly OwnStockTicker _stockTicker;

        public OwnRateHub() :
            this(OwnStockTicker.Instance)
        {

        }
        public void OwnOrderCompleted(string hash, long orderId, string symbol)
        {
            Clients.All.ownOrderCompleted(hash, orderId, symbol);
        }
        public OwnRateHub(OwnStockTicker stockTicker)
        {
            _stockTicker = stockTicker;

        }

        public void Start(string market)
        {
            _stockTicker.OpenOwnMarket(market, Context.ConnectionId);
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            _stockTicker.CloseOwnMarket(Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }
    }
}