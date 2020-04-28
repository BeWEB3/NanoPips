using Exchange.DTO;
using Exchange.Services.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Exchange.Common;
using Exchange.EF;
using System.Linq;
using System.Net;

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
                var val     = new WebClient().DownloadString("https://api.bittrex.com/v3/markets/" + market + "/candles?candleInterval=" + interval);
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
                string url = "https://api.binance.com/api/v3/klines?symbol=" + market + "&interval=" + interval + "&limit=" + limit;
                var val = new System.Net.WebClient().DownloadString(url);
                var candles = JsonConvert.DeserializeObject<object[][]>(val);
                return candles;
            }
            catch (Exception ex) {
                return null;
            }
        }

        public CandlesData GetCandleWebSocket(string market, string interval)
        {
            try
            {
                var dt =_db.CandlesDatas.Where(x => x.SymbolName == market && x.TimeFrameOriginal == interval).AsEnumerable().FirstOrDefault(x=> x.TimeFrameOriginal == interval);
                return dt;
            }
            catch (Exception ex) {
                return null;
            }
        }

        public GetPolygonCandles GetCandlesPolygon(string market, string from, string to, string interval)
        {
            try
            {
                var wC = new WebClient();
                var url = "https://api.polygon.io/v2/aggs/ticker/X:" + market + "/range/1/" + interval + "/" + from + "/" + to + "?unadjusted=true&apiKey=" + ApiContants.polygonApi;
                var val = wC.DownloadString(url);
                var candles = JsonConvert.DeserializeObject<GetPolygonCandles>(val);
                return candles;
            }
            catch (Exception ex)
            {
                var dt = new GetPolygonCandles();
                dt.results = new List<GetPolygonCandles.Ticker>();
                return dt;
            }
        }

        public GetLatestCandle GetLastCandlePolygon(string market, string from, string to, string interval)
        {
            try
            {
                string url  = "https://api.polygon.io/v2/snapshot/locale/global/markets/crypto/tickers/X:" + market + "?apiKey=" + ApiContants.polygonApi;
                var val     = new WebClient().DownloadString(url);
                var candles = JsonConvert.DeserializeObject<GetLatestCandle>(val);
                return candles;
            }
            catch (Exception ex) {
                var dt     = new GetLatestCandle();
                return dt;
            }
        }

        public OrderBookModel GetOrderBook(string market)
        {
            try
            {
                var val = new System.Net.WebClient().DownloadString("https://api.bittrex.com/v3/markets/" + market + "/orderbook");
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
                var val    = new System.Net.WebClient().DownloadString("https://api.bittrex.com/v3/markets/" + symbol + "/ticker");
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
                var val = new System.Net.WebClient().DownloadString("https://api.binance.com/api/v3/depth?symbol=" + symbol);
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
                var val = new System.Net.WebClient().DownloadString("https://api.binance.com/api/v3/ticker/bookTicker?symbol=" + symbol);
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
