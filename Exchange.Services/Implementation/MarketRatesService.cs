using Exchange.DTO;
using Exchange.Services.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using Exchange.Common;
using Exchange.EF;
using System.Linq;

namespace Exchange.Services.Implementation
{
    internal class MarketRatesService : IMarketRatesService
    {
        private ExchangeEntities _db;
        public MarketRatesService(ExchangeEntities db)
        {
            _db = db;
        }

        public List<Pair> GetAllPairs()
        {
            try {
                List<Pair> pairs = _db.Pairs.Where(x => x.Status == true).ToList();
                return pairs;
            }
            catch (Exception) { return null; }
        }

        public List<GetCandlesResponse> GetCandles(string market, string interval)
        {
            try
            {
                var val = new WebClient().DownloadString("https://api.bittrex.com/v3/markets/" + market + "/candles?candleInterval=" + interval);
                var candles = JsonConvert.DeserializeObject<List<GetCandlesResponse>>(val);
                return candles;
            }
            catch (Exception) {
                return new List<GetCandlesResponse>();
            }
        }

        public object GetTestCandles(string market, string interval, int limit)
        {
            try
            {
                var val = new WebClient().DownloadString("https://api.binance.com/api/v3/klines?symbol=" + market + "&interval=" + interval + "&limit=" + limit);
                var candles = JsonConvert.DeserializeObject<object[][]>(val);
                return candles;
            }
            catch (Exception ex) {
                return new object[0][];
            }
        }

        public OrderBookModel GetOrderBook(string market)
        {
            try
            {
                var val = new WebClient().DownloadString("https://api.bittrex.com/v3/markets/" + market + "/orderbook");
                var book = JsonConvert.DeserializeObject<OrderBookModel>(val);
                return book;
            }
            catch (Exception ex) {
                return null;
            }
        }

        public List<Currency> GetAllCurrencies() {
            try
            {
                return _db.Currencies.ToList();
            }
            catch (Exception) { return new List<Currency>(); }
        }

        public GetSpecificTicker GetSpecificTicker(string symbol)
        {
            try {
                var val    = new WebClient().DownloadString("https://api.bittrex.com/v3/markets/" + symbol + "/ticker");
                var ticker = JsonConvert.DeserializeObject<GetSpecificTicker>(val);
                return ticker;
            }
            catch (Exception ex) {
                return null;
            }
        }

        public bool ActivateCurrency(long curId)
        {
            _db.Currencies.Find(curId).Status = true;
            _db.Configuration.ValidateOnSaveEnabled = false;
            _db.SaveChanges();
            return true;
        }
        public bool DeactivateCurrency(long curId)
        {
            _db.Currencies.Find(curId).Status = false;
            _db.Configuration.ValidateOnSaveEnabled = false;
            _db.SaveChanges();
            return true;
        }

        public List<Currency> GetSpecificCurrency(string symbol)
        {
            try
            {
               return _db.Currencies.Where(x => x.ThreeDigitName == symbol).ToList();
            }
            catch (Exception) {
                return new List<Currency>();
            }
        }

        public GetOrderBookBinance GetOrderBookBinance(string symbol)
        {
            try {
                var val = new WebClient().DownloadString("https://api.binance.com/api/v3/depth?symbol=" + symbol);
                var book = JsonConvert.DeserializeObject<GetOrderBookBinance>(val);
                return book;
            }
            catch (Exception ex) {
                return null;
            }
        }

        public GetSpecificTickerBinance GetSpecificTickerBinance(string symbol)
        {
            try
            {
                var val = new WebClient().DownloadString("https://api.binance.com/api/v3/ticker/bookTicker?symbol=" + symbol);
                var ticker = JsonConvert.DeserializeObject<GetSpecificTickerBinance>(val);
                return ticker;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
