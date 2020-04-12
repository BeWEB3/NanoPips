using BinanceExchange.API.Client;
using BinanceExchange.API.Enums;
using BinanceExchange.API.Models.WebSocket;
using BinanceExchange.API.Websockets;
using Exchange.EF;
using System;
using System.Linq;
using System.Threading;

namespace WebSocketManagment
{
    public class Program
    {
        static void Main(string[] args)
        {
            BinanceKlineData Minute = null;
            BinanceKlineData FiveMinute = null;
            BinanceKlineData FifteenMinute = null;
            BinanceKlineData ThirtyMinute = null;
            BinanceKlineData Hour  = null;
            BinanceKlineData Day   = null;
            BinanceKlineData Month = null;
            BinanceKlineData Week  = null;

            BinanceKlineData MinuteETH = null;
            BinanceKlineData FiveMinuteETH = null;
            BinanceKlineData FifteenMinuteETH = null;
            BinanceKlineData ThirtyMinuteETH = null;
            BinanceKlineData HourETH  = null;
            BinanceKlineData DayETH   = null;
            BinanceKlineData MonthETH = null;
            BinanceKlineData WeekETH  = null;

            BinanceKlineData MinuteLTC = null;
            BinanceKlineData FiveMinuteLTC = null;
            BinanceKlineData FifteenMinuteLTC = null;
            BinanceKlineData ThirtyMinuteLTC = null;
            BinanceKlineData HourLTC  = null;
            BinanceKlineData DayLTC   = null;
            BinanceKlineData MonthLTC = null;
            BinanceKlineData WeekLTC  = null;

            BinanceKlineData MinuteXRP = null;
            BinanceKlineData FiveMinuteXRP = null;
            BinanceKlineData FifteenMinuteXRP = null;
            BinanceKlineData ThirtyMinuteXRP = null;
            BinanceKlineData HourXRP  = null;
            BinanceKlineData DayXRP   = null;
            BinanceKlineData MonthXRP = null;
            BinanceKlineData WeekXRP  = null;

            var client = new BinanceClient(new ClientConfiguration()
            {
                ApiKey = "9fc2M7WrNl6FdsEv9Pk80eGXy68bZgtUscp1oOPX6w2cnOKQyGdvrtoEAox9gQR2",
                SecretKey = "fS7RG3om3ChfRI2JpD5cV1Qj2ZWwIvG8ANtRlrluDqSRVmz6bzDgS3CR5T0qp2gf",
            });

            ///////////////////////////////////////////////////////////////
            Thread WebSocketSyncing = new Thread(() =>
            {
                try
                {
                    var manualWebSocketClient = new InstanceBinanceWebSocketClient(client);
                    var socketId = manualWebSocketClient.ConnectToKlineWebSocket("BTCUSDT", KlineInterval.OneMinute, data =>
                    {
                        Minute = data;
                    });

                    var manualWebSocketClient2 = new InstanceBinanceWebSocketClient(client);
                    var socketId2 = manualWebSocketClient2.ConnectToKlineWebSocket("BTCUSDT", KlineInterval.OneHour, data =>
                    {
                        Hour = data;
                    });

                    var manualWebSocketClient3 = new InstanceBinanceWebSocketClient(client);
                    var socketId3 = manualWebSocketClient2.ConnectToKlineWebSocket("BTCUSDT", KlineInterval.OneDay, data =>
                    {
                        Day = data;
                    });

                    //var manualWebSocketClient4 = new InstanceBinanceWebSocketClient(client);
                    //var socketId4 = manualWebSocketClient2.ConnectToKlineWebSocket("BTCUSDT", KlineInterval.OneMonth, data =>
                    //{
                    //    Month = data;
                    //});

                    var manualWebSocketClient5 = new InstanceBinanceWebSocketClient(client);
                    var socketId5 = manualWebSocketClient2.ConnectToKlineWebSocket("BTCUSDT", KlineInterval.OneWeek, data =>
                    {
                        Week = data;
                    });

                    var manualWebSocketClient20 = new InstanceBinanceWebSocketClient(client);
                    var socketId20 = manualWebSocketClient2.ConnectToKlineWebSocket("BTCUSDT", KlineInterval.FiveMinutes, data =>
                    {
                        FiveMinute = data;
                    });

                    var manualWebSocketClient21 = new InstanceBinanceWebSocketClient(client);
                    var socketId21 = manualWebSocketClient2.ConnectToKlineWebSocket("BTCUSDT", KlineInterval.FifteenMinutes, data =>
                    {
                        FifteenMinute = data;
                    });

                    var manualWebSocketClient22 = new InstanceBinanceWebSocketClient(client);
                    var socketId22 = manualWebSocketClient2.ConnectToKlineWebSocket("BTCUSDT", KlineInterval.ThirtyMinutes, data =>
                    {
                        ThirtyMinute = data;
                    });

                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    var manualWebSocketClient6 = new InstanceBinanceWebSocketClient(client);
                    var socketId6 = manualWebSocketClient.ConnectToKlineWebSocket("ETHUSDT", KlineInterval.OneMinute, data =>
                    {
                        MinuteETH = data;
                    });

                    var manualWebSocketClient7 = new InstanceBinanceWebSocketClient(client);
                    var socketId7 = manualWebSocketClient2.ConnectToKlineWebSocket("ETHUSDT", KlineInterval.OneHour, data =>
                    {
                        HourETH = data;
                    });

                    var manualWebSocketClient8 = new InstanceBinanceWebSocketClient(client);
                    var socketId8 = manualWebSocketClient2.ConnectToKlineWebSocket("ETHUSDT", KlineInterval.OneDay, data =>
                    {
                        DayETH = data;
                    });

                    //var manualWebSocketClient9 = new InstanceBinanceWebSocketClient(client);
                    //var socketId9 = manualWebSocketClient2.ConnectToKlineWebSocket("ETHUSDT", KlineInterval.OneMonth, data =>
                    //{
                    //    MonthETH = data;
                    //});

                    var manualWebSocketClient10 = new InstanceBinanceWebSocketClient(client);
                    var socketId10 = manualWebSocketClient2.ConnectToKlineWebSocket("ETHUSDT", KlineInterval.OneWeek, data =>
                    {
                        WeekETH = data;
                    });

                    var manualWebSocketClient23 = new InstanceBinanceWebSocketClient(client);
                    var socketId23 = manualWebSocketClient2.ConnectToKlineWebSocket("ETHUSDT", KlineInterval.FiveMinutes, data =>
                    {
                        FiveMinuteETH = data;
                    });

                    var manualWebSocketClient24 = new InstanceBinanceWebSocketClient(client);
                    var socketId24 = manualWebSocketClient2.ConnectToKlineWebSocket("ETHUSDT", KlineInterval.FifteenMinutes, data =>
                    {
                        FifteenMinuteETH = data;
                    });

                    var manualWebSocketClient25 = new InstanceBinanceWebSocketClient(client);
                    var socketId25 = manualWebSocketClient2.ConnectToKlineWebSocket("ETHUSDT", KlineInterval.ThirtyMinutes, data =>
                    {
                        ThirtyMinuteETH = data;
                    });

                    ////////////////////////////////  LTC-USDT  ////////////////////////////////////////
                    var manualWebSocketClient11 = new InstanceBinanceWebSocketClient(client);
                    var socketId11 = manualWebSocketClient.ConnectToKlineWebSocket("LTCUSDT", KlineInterval.OneMinute, data =>
                    {
                        MinuteLTC = data;
                    });

                    var manualWebSocketClient12 = new InstanceBinanceWebSocketClient(client);
                    var socketId12 = manualWebSocketClient2.ConnectToKlineWebSocket("LTCUSDT", KlineInterval.OneHour, data =>
                    {
                        HourLTC = data;
                    });

                    var manualWebSocketClient13 = new InstanceBinanceWebSocketClient(client);
                    var socketId13 = manualWebSocketClient2.ConnectToKlineWebSocket("LTCUSDT", KlineInterval.OneDay, data =>
                    {
                        DayLTC = data;
                    });

                    //var manualWebSocketClient14 = new InstanceBinanceWebSocketClient(client);
                    //var socketId14 = manualWebSocketClient2.ConnectToKlineWebSocket("LTCUSDT", KlineInterval.OneMonth, data =>
                    //{
                    //    MonthLTC = data;
                    //});

                    var manualWebSocketClient15 = new InstanceBinanceWebSocketClient(client);
                    var socketId15 = manualWebSocketClient2.ConnectToKlineWebSocket("LTCUSDT", KlineInterval.OneWeek, data =>
                    {
                        WeekLTC = data;
                    });

                    var manualWebSocketClient26 = new InstanceBinanceWebSocketClient(client);
                    var socketId26 = manualWebSocketClient2.ConnectToKlineWebSocket("LTCUSDT", KlineInterval.FiveMinutes, data =>
                    {
                        FiveMinuteLTC = data;
                    });

                    var manualWebSocketClient27 = new InstanceBinanceWebSocketClient(client);
                    var socketId27 = manualWebSocketClient2.ConnectToKlineWebSocket("LTCUSDT", KlineInterval.FifteenMinutes, data =>
                    {
                        FifteenMinuteLTC = data;
                    });

                    var manualWebSocketClient28 = new InstanceBinanceWebSocketClient(client);
                    var socketId28 = manualWebSocketClient2.ConnectToKlineWebSocket("LTCUSDT", KlineInterval.ThirtyMinutes, data =>
                    {
                        ThirtyMinuteLTC = data;
                    });


////////////////////////////////////////   XRP-USDT   ///////////////////////////////////////
                    var manualWebSocketClient16 = new InstanceBinanceWebSocketClient(client);
                    var socketId16 = manualWebSocketClient.ConnectToKlineWebSocket("XRPUSDT", KlineInterval.OneMinute, data =>
                    {
                        MinuteXRP = data;
                    });

                    var manualWebSocketClient17 = new InstanceBinanceWebSocketClient(client);
                    var socketId17 = manualWebSocketClient2.ConnectToKlineWebSocket("XRPUSDT", KlineInterval.OneHour, data =>
                    {
                        HourXRP = data;
                    });

                    var manualWebSocketClient18 = new InstanceBinanceWebSocketClient(client);
                    var socketId18 = manualWebSocketClient2.ConnectToKlineWebSocket("XRPUSDT", KlineInterval.OneDay, data =>
                    {
                        DayXRP = data;
                    });

                    //var manualWebSocketClient19 = new InstanceBinanceWebSocketClient(client);
                    //var socketId19 = manualWebSocketClient2.ConnectToKlineWebSocket("XRPUSDT", KlineInterval.OneMonth, data =>
                    //{
                    //    MonthXRP = data;
                    //});

                    var manualWebSocketClient33 = new InstanceBinanceWebSocketClient(client);
                    var socketId33 = manualWebSocketClient2.ConnectToKlineWebSocket("XRPUSDT", KlineInterval.OneWeek, data =>
                    {
                        WeekXRP = data;
                    });

                    var manualWebSocketClient29 = new InstanceBinanceWebSocketClient(client);
                    var socketId29 = manualWebSocketClient2.ConnectToKlineWebSocket("XRPUSDT", KlineInterval.FiveMinutes, data =>
                    {
                        FiveMinuteXRP = data;
                    });

                    var manualWebSocketClient30 = new InstanceBinanceWebSocketClient(client);
                    var socketId30 = manualWebSocketClient2.ConnectToKlineWebSocket("XRPUSDT", KlineInterval.FifteenMinutes, data =>
                    {
                        FifteenMinuteXRP = data;
                    });

                    var manualWebSocketClient31 = new InstanceBinanceWebSocketClient(client);
                    var socketId31 = manualWebSocketClient2.ConnectToKlineWebSocket("XRPUSDT", KlineInterval.ThirtyMinutes, data =>
                    {
                        ThirtyMinuteXRP = data;
                    });

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            });
            WebSocketSyncing.Start();

            //////////////////////////////////////////////////////////////
            Thread SetWebSocketData = new Thread (() =>
            {
                while (true)
                {
                    if (Minute != null && FiveMinute != null && FifteenMinute != null && ThirtyMinute != null && Hour != null && Day != null && Week != null
                         && MinuteETH != null && FiveMinuteETH != null && FifteenMinuteETH != null && ThirtyMinuteETH != null && HourETH != null && DayETH != null && WeekETH != null
                         && MinuteLTC != null && FiveMinuteLTC != null && FifteenMinuteLTC != null && ThirtyMinuteLTC != null && HourLTC != null && DayLTC != null && WeekLTC != null
                         && MinuteXRP != null && FiveMinuteXRP != null && FifteenMinuteXRP != null && ThirtyMinuteXRP != null && HourXRP != null && DayXRP != null && WeekXRP != null)
                    {

                        using (var db = new ExchangeEntities())
                        {
                            var candlesList = db.CandlesDatas.ToList();
                            for (int t = 0; t < candlesList.Count; t++)
                            {
                                var tmp = candlesList[t];
                                var dt = db.CandlesDatas.Where(x => x.SymbolName == tmp.SymbolName && x.TimeFrame == tmp.TimeFrame).FirstOrDefault();
                                BinanceKlineData commonDt = null;
                                try
                                {
                                    if (dt != null)
                                    {
                                        if (dt.SymbolName == "BTC-USDT" && dt.TimeFrame == "OneMinute") commonDt = Minute;
                                        else if (dt.SymbolName == "BTC-USDT" && dt.TimeFrame == "FiveMinute") commonDt    = FiveMinute;
                                        else if (dt.SymbolName == "BTC-USDT" && dt.TimeFrame == "FifteenMinute") commonDt = FifteenMinute;
                                        else if (dt.SymbolName == "BTC-USDT" && dt.TimeFrame == "ThirtyMinute") commonDt  = ThirtyMinute;
                                        else if (dt.SymbolName == "BTC-USDT" && dt.TimeFrame == "OneHour") commonDt = Hour;
                                        else if (dt.SymbolName == "BTC-USDT" && dt.TimeFrame == "OneDay") commonDt  = Day;
                                        else if (dt.SymbolName == "BTC-USDT" && dt.TimeFrame == "OneWeek") commonDt = Week;
                                        else if (dt.SymbolName == "ETH-USDT" && dt.TimeFrame == "OneMinute") commonDt = MinuteETH;
                                        else if (dt.SymbolName == "ETH-USDT" && dt.TimeFrame == "FiveMinute") commonDt = FiveMinuteETH;
                                        else if (dt.SymbolName == "ETH-USDT" && dt.TimeFrame == "FifteenMinute") commonDt = FifteenMinuteETH;
                                        else if (dt.SymbolName == "ETH-USDT" && dt.TimeFrame == "ThirtyMinute") commonDt = ThirtyMinuteETH;
                                        else if (dt.SymbolName == "ETH-USDT" && dt.TimeFrame == "OneHour") commonDt = HourETH;
                                        else if (dt.SymbolName == "ETH-USDT" && dt.TimeFrame == "OneDay") commonDt  = DayETH;
                                        else if (dt.SymbolName == "ETH-USDT" && dt.TimeFrame == "OneWeek") commonDt = WeekETH;
                                        else if (dt.SymbolName == "LTC-USDT" && dt.TimeFrame == "OneMinute") commonDt = MinuteLTC;
                                        else if (dt.SymbolName == "LTC-USDT" && dt.TimeFrame == "FiveMinute") commonDt = FiveMinuteLTC;
                                        else if (dt.SymbolName == "LTC-USDT" && dt.TimeFrame == "FifteenMinute") commonDt = FifteenMinuteLTC;
                                        else if (dt.SymbolName == "LTC-USDT" && dt.TimeFrame == "ThirtyMinute") commonDt = ThirtyMinuteLTC;
                                        else if (dt.SymbolName == "LTC-USDT" && dt.TimeFrame == "OneHour") commonDt = HourLTC;
                                        else if (dt.SymbolName == "LTC-USDT" && dt.TimeFrame == "OneDay") commonDt  = DayLTC;
                                        else if (dt.SymbolName == "LTC-USDT" && dt.TimeFrame == "OneWeek") commonDt = WeekLTC;
                                        else if (dt.SymbolName == "XRP-USDT" && dt.TimeFrame == "OneMinute") commonDt = MinuteXRP;
                                        else if (dt.SymbolName == "XRP-USDT" && dt.TimeFrame == "FiveMinute") commonDt = FiveMinuteXRP;
                                        else if (dt.SymbolName == "XRP-USDT" && dt.TimeFrame == "FifteenMinute") commonDt = FifteenMinuteXRP;
                                        else if (dt.SymbolName == "XRP-USDT" && dt.TimeFrame == "ThirtyMinute") commonDt = ThirtyMinuteXRP;
                                        else if (dt.SymbolName == "XRP-USDT" && dt.TimeFrame == "OneHour") commonDt = HourXRP;
                                        else if (dt.SymbolName == "XRP-USDT" && dt.TimeFrame == "OneDay") commonDt  = DayXRP;
                                        else if (dt.SymbolName == "XRP-USDT" && dt.TimeFrame == "OneWeek") commonDt = WeekXRP;

                                        DateTime javaEpoch = new DateTime(1970, 1, 1);

                                        dt.H = commonDt.Kline.High;
                                        dt.L = commonDt.Kline.Low;
                                        dt.O = commonDt.Kline.Open;
                                        dt.C = commonDt.Kline.Close;
                                        dt.Volume    = commonDt.Kline.Volume;
                                        dt.TimeStamp = (commonDt.Kline.StartTime.Ticks - javaEpoch.Ticks) / TimeSpan.TicksPerMillisecond;

                                        db.Configuration.ValidateOnSaveEnabled = false;
                                        db.SaveChanges();
                                    }
                                }
                                catch (Exception ex) { }
                            }
                        }
                        //Console.Clear();
                        //Console.WriteLine("      *** BTC-USDT Candle *** "); Console.WriteLine();
                        //Console.WriteLine("MINUTE:  " + "HIGH: " + Minute.Kline.High + "  LOW: " + Minute.Kline.Low + "  OPEN: " + Minute.Kline.Open + "  CLOSE: " + Minute.Kline.Close); Console.WriteLine();
                        //Console.WriteLine("HOUR:    " + "HIGH: " + Hour.Kline.High + "  LOW: " + Hour.Kline.Low + "  OPEN: " + Hour.Kline.Open + "  CLOSE: " + Hour.Kline.Close); Console.WriteLine();
                        //Console.WriteLine("DAY:     " + "HIGH: " + Day.Kline.High + "  LOW: " + Day.Kline.Low + "  OPEN: " + Day.Kline.Open + "  CLOSE: " + Day.Kline.Close); Console.WriteLine();
                        //Console.WriteLine("WEEK:    " + "HIGH: " + Week.Kline.High + "  LOW: " + Week.Kline.Low + "  OPEN: " + Week.Kline.Open + "  CLOSE: " + Week.Kline.Close); Console.WriteLine();
                        //Console.WriteLine("MONTH:   " + "HIGH: " + Month.Kline.High + "  LOW: " + Month.Kline.Low + "  OPEN: " + Month.Kline.Open + "  CLOSE: " + Month.Kline.Close); Console.WriteLine();
                        //Console.WriteLine("      *** ETH-USDT Candle *** "); Console.WriteLine();
                        //Console.WriteLine("MINUTE:  " + "HIGH: " + MinuteETH.Kline.High + "  LOW: " + MinuteETH.Kline.Low + "  OPEN: " + MinuteETH.Kline.Open + "  CLOSE: " + MinuteETH.Kline.Close); Console.WriteLine();
                        //Console.WriteLine("HOUR:    " + "HIGH: " + HourETH.Kline.High + "  LOW: " + HourETH.Kline.Low + "  OPEN: " + HourETH.Kline.Open + "  CLOSE: " + HourETH.Kline.Close); Console.WriteLine();
                        //Console.WriteLine("DAY:     " + "HIGH: " + DayETH.Kline.High + "  LOW: " + DayETH.Kline.Low + "  OPEN: " + DayETH.Kline.Open + "  CLOSE: " + DayETH.Kline.Close); Console.WriteLine();
                        //Console.WriteLine("WEEK:    " + "HIGH: " + WeekETH.Kline.High + "  LOW: " + WeekETH.Kline.Low + "  OPEN: " + WeekETH.Kline.Open + "  CLOSE: " + WeekETH.Kline.Close); Console.WriteLine();
                        //Console.WriteLine("MONTH:   " + "HIGH: " + MonthETH.Kline.High + "  LOW: " + MonthETH.Kline.Low + "  OPEN: " + MonthETH.Kline.Open + "  CLOSE: " + MonthETH.Kline.Close); Console.WriteLine();
                        //Console.WriteLine("      *** LTC-USDT Candle *** "); Console.WriteLine();
                        //Console.WriteLine("MINUTE:  " + "HIGH: " + MinuteLTC.Kline.High + "  LOW: " + MinuteLTC.Kline.Low + "  OPEN: " + MinuteLTC.Kline.Open + "  CLOSE: " + MinuteLTC.Kline.Close); Console.WriteLine();
                        //Console.WriteLine("HOUR:    " + "HIGH: " + HourLTC.Kline.High + "  LOW: " + HourLTC.Kline.Low + "  OPEN: " + HourLTC.Kline.Open + "  CLOSE: " + HourLTC.Kline.Close); Console.WriteLine();
                        //Console.WriteLine("DAY:     " + "HIGH: " + DayLTC.Kline.High + "  LOW: " + DayLTC.Kline.Low + "  OPEN: " + DayLTC.Kline.Open + "  CLOSE: " + DayLTC.Kline.Close); Console.WriteLine();
                        //Console.WriteLine("WEEK:    " + "HIGH: " + WeekLTC.Kline.High + "  LOW: " + WeekLTC.Kline.Low + "  OPEN: " + WeekLTC.Kline.Open + "  CLOSE: " + WeekLTC.Kline.Close); Console.WriteLine();
                        //Console.WriteLine("MONTH:   " + "HIGH: " + MonthLTC.Kline.High + "  LOW: " + MonthLTC.Kline.Low + "  OPEN: " + MonthLTC.Kline.Open + "  CLOSE: " + MonthLTC.Kline.Close); Console.WriteLine();
                        Thread.Sleep(900);
                    }
                }
            });
            SetWebSocketData.Start();
        }

        //public static long UnixTimeStampToDateTime(long unixTimeStamp)
        //{
        //    System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
        //    var xa = dtDateTime.AddSeconds(unixTimeStamp).ToUniversalTime().Ticks;
        //    return xa;
        //}

    }
}
