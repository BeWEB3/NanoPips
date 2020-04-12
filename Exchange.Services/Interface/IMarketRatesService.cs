using Exchange.Common;
using Exchange.DTO;
using System;
using System.Collections.Generic;

namespace Exchange.Services.Interface
{
    public interface IMarketRatesService
    {
        List<GetCandlesResponse> GetCandles(string market, string interval);
        GetSpecificTicker GetSpecificTicker(string symbol);
        GetSpecificTickerBinance GetSpecificTickerBinance(string symbol);
        OrderBookModel GetOrderBook(string symbol);
        GetOrderBookBinance GetOrderBookBinance(string symbol);
        List<Currency> GetAllCurrencies();
        List<Currency> GetSpecificCurrency(string symbol);
        List<Pair> GetAllPairs();
        bool ActivateCurrency(long curId);
        bool DeactivateCurrency(long curId);
        object GetTestCandles(string market, string interval, int limit);
        GetPolygonCandles GetCandlesPolygon(string market, string from, string to, string interval);
        GetLatestCandle   GetLastCandlePolygon(string market, string from, string to, string interval);
        CandlesData GetCandleWebSocket(string market, string interval);

    }
}
