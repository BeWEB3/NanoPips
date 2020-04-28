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

            BinanceKlineData MinuteETH = null;
            BinanceKlineData FiveMinuteETH = null;
            BinanceKlineData FifteenMinuteETH = null;
            BinanceKlineData ThirtyMinuteETH = null;
            BinanceKlineData HourETH  = null;
            BinanceKlineData DayETH   = null;

            BinanceKlineData MinuteLTC = null;
            BinanceKlineData FiveMinuteLTC = null;
            BinanceKlineData FifteenMinuteLTC = null;
            BinanceKlineData ThirtyMinuteLTC = null;
            BinanceKlineData HourLTC  = null;
            BinanceKlineData DayLTC   = null;

            BinanceKlineData MinuteXRP = null;
            BinanceKlineData FiveMinuteXRP = null;
            BinanceKlineData FifteenMinuteXRP = null;
            BinanceKlineData ThirtyMinuteXRP = null;
            BinanceKlineData HourXRP  = null;
            BinanceKlineData DayXRP   = null;

            /////////////// Main ////////////////////
            var ApiKey    = "TdRGlhtg7RsuQG6MPmbKvasd2EOMFBopLIVLmeAsoIRq5Q03qeCZwAOUCqjLhYIO";
            var SecretKey = "45gClX8cfTo2zUezse6ETFdH1kMO44OnqdgaZEHve3nu1vRLWpI26ku52yjS9odt";

            //////////////// Test ////////////////////
            //var ApiKey = "9fc2M7WrNl6FdsEv9Pk80eGXy68bZgtUscp1oOPX6w2cnOKQyGdvrtoEAox9gQR2";
            //var SecretKey = "fS7RG3om3ChfRI2JpD5cV1Qj2ZWwIvG8ANtRlrluDqSRVmz6bzDgS3CR5T0qp2gf";

            var client = new BinanceClient(new ClientConfiguration()
            {
                ApiKey = ApiKey,
                SecretKey = SecretKey,
            });

            var client1 = new BinanceClient(new ClientConfiguration()
            {
                ApiKey = ApiKey,
                SecretKey = SecretKey,
            });

            var client2 = new BinanceClient(new ClientConfiguration()
            {
                ApiKey = ApiKey,
                SecretKey = SecretKey,
            });

            var client3 = new BinanceClient(new ClientConfiguration()
            {
                ApiKey = ApiKey,
                SecretKey = SecretKey,
            });

            var client4 = new BinanceClient(new ClientConfiguration()
            {
                ApiKey = ApiKey,
                SecretKey = SecretKey,
            });

            var client5 = new BinanceClient(new ClientConfiguration()
            {
                ApiKey = ApiKey,
                SecretKey = SecretKey,
            });

            var client6 = new BinanceClient(new ClientConfiguration()
            {
                ApiKey = ApiKey,
                SecretKey = SecretKey,
            });

            var client7 = new BinanceClient(new ClientConfiguration()
            {
                ApiKey = ApiKey,
                SecretKey = SecretKey,
            });

            var client8 = new BinanceClient(new ClientConfiguration()
            {
                ApiKey = ApiKey,
                SecretKey = SecretKey,
            });

            var client9 = new BinanceClient(new ClientConfiguration()
            {
                ApiKey = ApiKey,
                SecretKey = SecretKey,
            });

            var client10 = new BinanceClient(new ClientConfiguration()
            {
                ApiKey = ApiKey,
                SecretKey = SecretKey,
            });

            var client11 = new BinanceClient(new ClientConfiguration()
            {
                ApiKey = ApiKey,
                SecretKey = SecretKey,
            });

            var client12 = new BinanceClient(new ClientConfiguration()
            {
                ApiKey = ApiKey,
                SecretKey = SecretKey,
            });

            var client13 = new BinanceClient(new ClientConfiguration()
            {
                ApiKey = ApiKey,
                SecretKey = SecretKey,
            });

            var client14 = new BinanceClient(new ClientConfiguration()
            {
                ApiKey = ApiKey,
                SecretKey = SecretKey,
            });

            var client15 = new BinanceClient(new ClientConfiguration()
            {
                ApiKey = ApiKey,
                SecretKey = SecretKey,
            });

            var client16 = new BinanceClient(new ClientConfiguration()
            {
                ApiKey = ApiKey,
                SecretKey = SecretKey,
            });

            var client17 = new BinanceClient(new ClientConfiguration()
            {
                ApiKey = ApiKey,
                SecretKey = SecretKey,
            });

            var client18 = new BinanceClient(new ClientConfiguration()
            {
                ApiKey = ApiKey,
                SecretKey = SecretKey,
            });

            var client19 = new BinanceClient(new ClientConfiguration()
            {
                ApiKey = ApiKey,
                SecretKey = SecretKey,
            });

            var client20 = new BinanceClient(new ClientConfiguration()
            {
                ApiKey = ApiKey,
                SecretKey = SecretKey,
            });

            var client21 = new BinanceClient(new ClientConfiguration()
            {
                ApiKey = ApiKey,
                SecretKey = SecretKey,
            });

            var client22 = new BinanceClient(new ClientConfiguration()
            {
                ApiKey = ApiKey,
                SecretKey = SecretKey,
            });

            var client23 = new BinanceClient(new ClientConfiguration()
            {
                ApiKey = ApiKey,
                SecretKey = SecretKey,
            });

            Console.WriteLine("WebSocket Syncing Start...");
            ///////////////////////////////////////////////////////////////
            Thread WebSocketSyncing = new Thread(() =>
            {
                var manualWebSocketClient   = new InstanceBinanceWebSocketClient(client);
                var manualWebSocketClient2  = new InstanceBinanceWebSocketClient(client1);
                var manualWebSocketClient20 = new InstanceBinanceWebSocketClient(client2);
                var manualWebSocketClient3  = new InstanceBinanceWebSocketClient(client3);
                var manualWebSocketClient21 = new InstanceBinanceWebSocketClient(client4);
                var manualWebSocketClient22 = new InstanceBinanceWebSocketClient(client5);
                var manualWebSocketClient6  = new InstanceBinanceWebSocketClient(client6);
                var manualWebSocketClient7  = new InstanceBinanceWebSocketClient(client7);
                var manualWebSocketClient8  = new InstanceBinanceWebSocketClient(client8);
                var manualWebSocketClient23 = new InstanceBinanceWebSocketClient(client9);
                var manualWebSocketClient24 = new InstanceBinanceWebSocketClient(client10);
                var manualWebSocketClient25 = new InstanceBinanceWebSocketClient(client11);
                var manualWebSocketClient11 = new InstanceBinanceWebSocketClient(client12);
                var manualWebSocketClient12 = new InstanceBinanceWebSocketClient(client13);
                var manualWebSocketClient13 = new InstanceBinanceWebSocketClient(client14);
                var manualWebSocketClient26 = new InstanceBinanceWebSocketClient(client15);
                var manualWebSocketClient27 = new InstanceBinanceWebSocketClient(client16);
                var manualWebSocketClient28 = new InstanceBinanceWebSocketClient(client17);
                var manualWebSocketClient16 = new InstanceBinanceWebSocketClient(client18);
                var manualWebSocketClient17 = new InstanceBinanceWebSocketClient(client19);
                var manualWebSocketClient18 = new InstanceBinanceWebSocketClient(client20);
                var manualWebSocketClient29 = new InstanceBinanceWebSocketClient(client21);
                var manualWebSocketClient30 = new InstanceBinanceWebSocketClient(client22);
                var manualWebSocketClient31 = new InstanceBinanceWebSocketClient(client23);

                    try
                    {
                        var socketId = manualWebSocketClient.ConnectToKlineWebSocket("BTCUSDT", KlineInterval.OneMinute, data =>
                        {
                            try
                            {
                                Minute = data;
                            }
                            catch (Exception) {
                                Environment.Exit(0);
                            }
                        });

                        var socketId2 = manualWebSocketClient2.ConnectToKlineWebSocket("BTCUSDT", KlineInterval.OneHour, data =>
                        {
                            try
                            {
                                Hour = data;
                            }
                            catch (Exception)
                            {
                                Environment.Exit(0);
                            }
                        });

                        var socketId3 = manualWebSocketClient3.ConnectToKlineWebSocket("BTCUSDT", KlineInterval.OneDay, data =>
                        {
                            try { 
                                Day = data;
                            }
                            catch (Exception)
                            {
                                Environment.Exit(0);
                            }
                        });

                        var socketId20 = manualWebSocketClient20.ConnectToKlineWebSocket("BTCUSDT", KlineInterval.FiveMinutes, data =>
                        {
                            try { 
                                FiveMinute = data;
                            }
                            catch (Exception)
                            {
                                Environment.Exit(0);
                            }
                        });

                        var socketId21 = manualWebSocketClient21.ConnectToKlineWebSocket("BTCUSDT", KlineInterval.FifteenMinutes, data =>
                        {
                            try { 
                                FifteenMinute = data;
                            }
                            catch (Exception)
                            {
                                Environment.Exit(0);
                            }
                        });

                        var socketId22 = manualWebSocketClient22.ConnectToKlineWebSocket("BTCUSDT", KlineInterval.ThirtyMinutes, data =>
                        {
                            try { 
                                ThirtyMinute = data;
                            }
                            catch (Exception)
                            {
                                Environment.Exit(0);
                            }
                        });

                        /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        var socketId6 = manualWebSocketClient6.ConnectToKlineWebSocket("ETHUSDT", KlineInterval.OneMinute, data =>
                        {
                            try { 
                                MinuteETH = data;
                            }
                            catch (Exception)
                            {
                                Environment.Exit(0);
                            }
                        });

                        var socketId7 = manualWebSocketClient7.ConnectToKlineWebSocket("ETHUSDT", KlineInterval.OneHour, data =>
                        {
                            try { 
                                HourETH = data;
                            }
                            catch (Exception)
                            {
                                Environment.Exit(0);
                            }
                        });

                        var socketId8 = manualWebSocketClient8.ConnectToKlineWebSocket("ETHUSDT", KlineInterval.OneDay, data =>
                        {
                            try { 
                                DayETH = data;
                            }
                            catch (Exception)
                            {
                                Environment.Exit(0);
                            }
                        });

                        var socketId23 = manualWebSocketClient23.ConnectToKlineWebSocket("ETHUSDT", KlineInterval.FiveMinutes, data =>
                        {
                            try { 
                                FiveMinuteETH = data;
                            }
                            catch (Exception)
                            {
                                Environment.Exit(0);
                            }
                        });

                        var socketId24 = manualWebSocketClient24.ConnectToKlineWebSocket("ETHUSDT", KlineInterval.FifteenMinutes, data =>
                        {
                            try { 
                                FifteenMinuteETH = data;
                            }
                            catch (Exception)
                            {
                                Environment.Exit(0);
                            }
                        });

                        var socketId25 = manualWebSocketClient25.ConnectToKlineWebSocket("ETHUSDT", KlineInterval.ThirtyMinutes, data =>
                        {
                            try { 
                                ThirtyMinuteETH = data;
                            }
                            catch (Exception)
                            {
                                Environment.Exit(0);
                            }
                        });

                        ////////////////////////////////  LTC-USDT  ////////////////////////////////////////
                        var socketId11 = manualWebSocketClient11.ConnectToKlineWebSocket("LTCUSDT", KlineInterval.OneMinute, data =>
                        {
                            try { 
                                MinuteLTC = data;
                            }
                            catch (Exception)
                            {
                                Environment.Exit(0);
                            }
                        });

                        var socketId12 = manualWebSocketClient12.ConnectToKlineWebSocket("LTCUSDT", KlineInterval.OneHour, data =>
                        {
                            try { 
                                HourLTC = data;
                            }
                            catch (Exception)
                            {
                                Environment.Exit(0);
                            }
                        });

                        var socketId13 = manualWebSocketClient13.ConnectToKlineWebSocket("LTCUSDT", KlineInterval.OneDay, data =>
                        {
                            try { 
                                DayLTC = data;
                            }
                            catch (Exception)
                            {
                                Environment.Exit(0);
                            }
                        });

                        var socketId26 = manualWebSocketClient26.ConnectToKlineWebSocket("LTCUSDT", KlineInterval.FiveMinutes, data =>
                        {
                            try { 
                                FiveMinuteLTC = data;
                            }
                            catch (Exception)
                            {
                                Environment.Exit(0);
                            }
                        });

                        var socketId27 = manualWebSocketClient27.ConnectToKlineWebSocket("LTCUSDT", KlineInterval.FifteenMinutes, data =>
                        {
                            try { 
                                FifteenMinuteLTC = data;
                            }
                            catch (Exception)
                            {
                                Environment.Exit(0);
                            }
                        });

                        var socketId28 = manualWebSocketClient28.ConnectToKlineWebSocket("LTCUSDT", KlineInterval.ThirtyMinutes, data =>
                        {
                            try
                            {
                                ThirtyMinuteLTC = data;
                            }
                            catch (Exception)
                            {
                                Environment.Exit(0);
                            }
                        });

                        ////////////////////////////////////////   XRP-USDT   ///////////////////////////////////////
                        var socketId16 = manualWebSocketClient16.ConnectToKlineWebSocket("XRPUSDT", KlineInterval.OneMinute, data =>
                        {
                            try { 
                                MinuteXRP = data;
                            }
                            catch (Exception)
                            {
                                Environment.Exit(0);
                            }
                        });

                        var socketId17 = manualWebSocketClient17.ConnectToKlineWebSocket("XRPUSDT", KlineInterval.OneHour, data =>
                        {
                            try { 
                                HourXRP = data;
                            }
                            catch (Exception)
                            {
                                Environment.Exit(0);
                            }
                        });

                        var socketId18 = manualWebSocketClient18.ConnectToKlineWebSocket("XRPUSDT", KlineInterval.OneDay, data =>
                        {
                            try { 
                                DayXRP = data;
                            }
                            catch (Exception)
                            {
                                Environment.Exit(0);
                            }
                        });

                        var socketId29 = manualWebSocketClient29.ConnectToKlineWebSocket("XRPUSDT", KlineInterval.FiveMinutes, data =>
                        {
                            try { 
                                FiveMinuteXRP = data;
                            }
                            catch (Exception)
                            {
                                Environment.Exit(0);
                            }
                        });

                        var socketId30 = manualWebSocketClient30.ConnectToKlineWebSocket("XRPUSDT", KlineInterval.FifteenMinutes, data =>
                        {
                            try { 
                                FifteenMinuteXRP = data;
                            }
                            catch (Exception)
                            {
                                Environment.Exit(0);
                            }
                        });

                        var socketId31 = manualWebSocketClient31.ConnectToKlineWebSocket("XRPUSDT", KlineInterval.ThirtyMinutes, data =>
                        {
                            try { 
                                ThirtyMinuteXRP = data;
                            }
                            catch (Exception)
                            {
                                Environment.Exit(0);
                            }
                        });

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("ERROR ON SOCKET:  " + ex.Message);
                        Environment.Exit(0);
                }
            });
            WebSocketSyncing.Start();

                //////////////////////////////////////////////////////////////
            Thread SetWebSocketData = new Thread (() =>
            {
                while (true)
                {
                    if (Minute != null && FiveMinute != null && FifteenMinute != null && ThirtyMinute != null && Hour != null && Day != null
                         && MinuteETH != null && FiveMinuteETH != null && FifteenMinuteETH != null && ThirtyMinuteETH != null && HourETH != null && DayETH != null
                         && MinuteLTC != null && FiveMinuteLTC != null && FifteenMinuteLTC != null && ThirtyMinuteLTC != null && HourLTC != null && DayLTC != null
                         && MinuteXRP != null && FiveMinuteXRP != null && FifteenMinuteXRP != null && ThirtyMinuteXRP != null && HourXRP != null && DayXRP != null)
                    {
                        using (var db = new ExchangeEntities())
                        {
                            var candlesList = db.CandlesDatas.ToList();
                            for (int t = 0; t < candlesList.Count; t++)
                            {
                                if (candlesList[t].Status.Value)
                                {
                                    var tmp = candlesList[t];
                                    var dt = db.CandlesDatas.Where(x => x.SymbolName == tmp.SymbolName && x.TimeFrame == tmp.TimeFrame).FirstOrDefault();
                                    BinanceKlineData commonDt = null;
                                    try
                                    {
                                        if (dt != null)
                                        {
                                            if (dt.SymbolName == "BTC-USDT" && dt.TimeFrame == "OneMinute") commonDt = Minute;
                                            else if (dt.SymbolName == "BTC-USDT" && dt.TimeFrame == "FiveMinute") commonDt = FiveMinute;
                                            else if (dt.SymbolName == "BTC-USDT" && dt.TimeFrame == "FifteenMinute") commonDt = FifteenMinute;
                                            else if (dt.SymbolName == "BTC-USDT" && dt.TimeFrame == "ThirtyMinute") commonDt = ThirtyMinute;
                                            else if (dt.SymbolName == "BTC-USDT" && dt.TimeFrame == "OneHour") commonDt = Hour;
                                            else if (dt.SymbolName == "BTC-USDT" && dt.TimeFrame == "OneDay") commonDt = Day;
                                            else if (dt.SymbolName == "ETH-USDT" && dt.TimeFrame == "OneMinute") commonDt = MinuteETH;
                                            else if (dt.SymbolName == "ETH-USDT" && dt.TimeFrame == "FiveMinute") commonDt = FiveMinuteETH;
                                            else if (dt.SymbolName == "ETH-USDT" && dt.TimeFrame == "FifteenMinute") commonDt = FifteenMinuteETH;
                                            else if (dt.SymbolName == "ETH-USDT" && dt.TimeFrame == "ThirtyMinute") commonDt = ThirtyMinuteETH;
                                            else if (dt.SymbolName == "ETH-USDT" && dt.TimeFrame == "OneHour") commonDt = HourETH;
                                            else if (dt.SymbolName == "ETH-USDT" && dt.TimeFrame == "OneDay") commonDt = DayETH;
                                            else if (dt.SymbolName == "LTC-USDT" && dt.TimeFrame == "OneMinute") commonDt = MinuteLTC;
                                            else if (dt.SymbolName == "LTC-USDT" && dt.TimeFrame == "FiveMinute") commonDt = FiveMinuteLTC;
                                            else if (dt.SymbolName == "LTC-USDT" && dt.TimeFrame == "FifteenMinute") commonDt = FifteenMinuteLTC;
                                            else if (dt.SymbolName == "LTC-USDT" && dt.TimeFrame == "ThirtyMinute") commonDt = ThirtyMinuteLTC;
                                            else if (dt.SymbolName == "LTC-USDT" && dt.TimeFrame == "OneHour") commonDt = HourLTC;
                                            else if (dt.SymbolName == "LTC-USDT" && dt.TimeFrame == "OneDay") commonDt = DayLTC;
                                            else if (dt.SymbolName == "XRP-USDT" && dt.TimeFrame == "OneMinute") commonDt = MinuteXRP;
                                            else if (dt.SymbolName == "XRP-USDT" && dt.TimeFrame == "FiveMinute") commonDt = FiveMinuteXRP;
                                            else if (dt.SymbolName == "XRP-USDT" && dt.TimeFrame == "FifteenMinute") commonDt = FifteenMinuteXRP;
                                            else if (dt.SymbolName == "XRP-USDT" && dt.TimeFrame == "ThirtyMinute") commonDt = ThirtyMinuteXRP;
                                            else if (dt.SymbolName == "XRP-USDT" && dt.TimeFrame == "OneHour") commonDt = HourXRP;
                                            else if (dt.SymbolName == "XRP-USDT" && dt.TimeFrame == "OneDay") commonDt = DayXRP;

                                            DateTime javaEpoch = new DateTime(1970, 1, 1);

                                            dt.H = commonDt.Kline.High;
                                            dt.L = commonDt.Kline.Low;
                                            dt.O = commonDt.Kline.Open;
                                            dt.C = commonDt.Kline.Close;
                                            dt.Volume = commonDt.Kline.Volume;
                                            dt.TimeStamp = (commonDt.Kline.StartTime.Ticks - javaEpoch.Ticks) / TimeSpan.TicksPerMillisecond;

                                            db.Configuration.ValidateOnSaveEnabled = false;
                                            db.SaveChanges();
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine("ERROR ON SAVING:  " + ex.Message);
                                    }
                                }
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

        //Boolean check = manualWebSocketClient.IsAlive(socketId);

        //catch (BinanceBadRequestException badRequestException)
        //{ }
        //catch (BinanceServerException serverException)
        //{ }
        //catch (BinanceTimeoutException timeoutException)
        //{ }
        //catch (BinanceException unknownException)
        //{ }

    }
}
