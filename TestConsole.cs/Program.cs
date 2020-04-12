using BinanceExchange.API.Client;
using BinanceExchange.API.Enums;
using BinanceExchange.API.Models.WebSocket;
using BinanceExchange.API.Websockets;
using Newtonsoft.Json;
using PolygonWeb;
using System;
using System.Threading;

namespace TestConsole.cs
{
    class Program
    {
        static void Main(string[] args)
        {
            var obj = new PolygonWebConnect();
            obj.Start();

            //            BinanceKlineData Minute = null;
            //            BinanceKlineData Hour   = null;
            //            BinanceKlineData Day    = null;
            //            BinanceKlineData Month  = null;
            //            BinanceKlineData Week   = null;

            //            BinanceKlineData MinuteETH = null;
            //            BinanceKlineData HourETH = null;
            //            BinanceKlineData DayETH = null;
            //            BinanceKlineData MonthETH = null;
            //            BinanceKlineData WeekETH = null;

            //            BinanceKlineData MinuteLTC = null;
            //            BinanceKlineData HourLTC = null;
            //            BinanceKlineData DayLTC = null;
            //            BinanceKlineData MonthLTC = null;
            //            BinanceKlineData WeekLTC = null;

            //            var client = new BinanceClient(new ClientConfiguration()
            //            {
            //                ApiKey    = "9fc2M7WrNl6FdsEv9Pk80eGXy68bZgtUscp1oOPX6w2cnOKQyGdvrtoEAox9gQR2",
            //                SecretKey = "fS7RG3om3ChfRI2JpD5cV1Qj2ZWwIvG8ANtRlrluDqSRVmz6bzDgS3CR5T0qp2gf",
            //            });

            //            var manualWebSocketClient = new InstanceBinanceWebSocketClient(client);
            //            var socketId = manualWebSocketClient.ConnectToKlineWebSocket("BTCUSDT", KlineInterval.OneMinute, data =>
            //            {
            //                Minute = data;
            //            });

            //            var manualWebSocketClient2 = new InstanceBinanceWebSocketClient(client);
            //            var socketId2 = manualWebSocketClient2.ConnectToKlineWebSocket("BTCUSDT", KlineInterval.OneHour, data =>
            //            {
            //                Hour = data;
            //            });

            //            var manualWebSocketClient3 = new InstanceBinanceWebSocketClient(client);
            //            var socketId3 = manualWebSocketClient2.ConnectToKlineWebSocket("BTCUSDT", KlineInterval.OneDay, data =>
            //            {
            //                Day = data;
            //            });

            //            var manualWebSocketClient4 = new InstanceBinanceWebSocketClient(client);
            //            var socketId4 = manualWebSocketClient2.ConnectToKlineWebSocket("BTCUSDT", KlineInterval.OneMonth, data =>
            //            {
            //                Month = data;
            //            });

            //            var manualWebSocketClient5 = new InstanceBinanceWebSocketClient(client);
            //            var socketId5 = manualWebSocketClient2.ConnectToKlineWebSocket("BTCUSDT", KlineInterval.OneWeek, data =>
            //            {
            //                Week = data;
            //            });
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //            var manualWebSocketClient6 = new InstanceBinanceWebSocketClient(client);
            //            var socketId6 = manualWebSocketClient.ConnectToKlineWebSocket("ETHUSDT", KlineInterval.OneMinute, data =>
            //            {
            //                MinuteETH = data;
            //            });

            //            var manualWebSocketClient7 = new InstanceBinanceWebSocketClient(client);
            //            var socketId7 = manualWebSocketClient2.ConnectToKlineWebSocket("ETHUSDT", KlineInterval.OneHour, data =>
            //            {
            //                HourETH = data;
            //            });

            //            var manualWebSocketClient8 = new InstanceBinanceWebSocketClient(client);
            //            var socketId8 = manualWebSocketClient2.ConnectToKlineWebSocket("ETHUSDT", KlineInterval.OneDay, data =>
            //            {
            //                DayETH = data;
            //            });

            //            var manualWebSocketClient9 = new InstanceBinanceWebSocketClient(client);
            //            var socketId9 = manualWebSocketClient2.ConnectToKlineWebSocket("ETHUSDT", KlineInterval.OneMonth, data =>
            //            {
            //                MonthETH = data;
            //            });

            //            var manualWebSocketClient10 = new InstanceBinanceWebSocketClient(client);
            //            var socketId10 = manualWebSocketClient2.ConnectToKlineWebSocket("ETHUSDT", KlineInterval.OneWeek, data =>
            //            {
            //                WeekETH = data;
            //            });
            //////////////////////////////////////////////////////////////////////////
            //            var manualWebSocketClient11 = new InstanceBinanceWebSocketClient(client);
            //            var socketId11 = manualWebSocketClient.ConnectToKlineWebSocket("LTCUSDT", KlineInterval.OneMinute, data =>
            //            {
            //                MinuteLTC = data;
            //            });

            //            var manualWebSocketClient12 = new InstanceBinanceWebSocketClient(client);
            //            var socketId12 = manualWebSocketClient2.ConnectToKlineWebSocket("LTCUSDT", KlineInterval.OneHour, data =>
            //            {
            //                HourLTC = data;
            //            });

            //            var manualWebSocketClient13 = new InstanceBinanceWebSocketClient(client);
            //            var socketId13 = manualWebSocketClient2.ConnectToKlineWebSocket("LTCUSDT", KlineInterval.OneDay, data =>
            //            {
            //                DayLTC = data;
            //            });

            //            var manualWebSocketClient14 = new InstanceBinanceWebSocketClient(client);
            //            var socketId14 = manualWebSocketClient2.ConnectToKlineWebSocket("LTCUSDT", KlineInterval.OneMonth, data =>
            //            {
            //                MonthLTC = data;
            //            });

            //            var manualWebSocketClient15 = new InstanceBinanceWebSocketClient(client);
            //            var socketId15 = manualWebSocketClient2.ConnectToKlineWebSocket("LTCUSDT", KlineInterval.OneWeek, data =>
            //            {
            //                WeekLTC = data;
            //            });

            //            while (true) {
            //                if (Minute != null && Hour != null && Day != null && Month != null && Week != null
            //                     && MinuteETH != null && HourETH != null && DayETH != null && MonthETH != null && WeekETH != null
            //                     && MinuteLTC != null && HourLTC != null && DayLTC != null && MonthLTC != null && WeekLTC != null)
            //                {
            //                    Console.WriteLine("      *** BTC-USDT Candle *** "); Console.WriteLine();
            //                    Console.WriteLine("MINUTE:  " + "HIGH: " + Minute.Kline.High + "  LOW: " + Minute.Kline.Low + "  OPEN: " + Minute.Kline.Open + "  CLOSE: " + Minute.Kline.Close); Console.WriteLine();
            //                    Console.WriteLine("HOUR:    " + "HIGH: " + Hour.Kline.High + "  LOW: " + Hour.Kline.Low + "  OPEN: " + Hour.Kline.Open + "  CLOSE: " + Hour.Kline.Close); Console.WriteLine();
            //                    Console.WriteLine("DAY:     " + "HIGH: " + Day.Kline.High + "  LOW: " + Day.Kline.Low + "  OPEN: " + Day.Kline.Open + "  CLOSE: " + Day.Kline.Close); Console.WriteLine();
            //                    Console.WriteLine("WEEK:    " + "HIGH: " + Week.Kline.High + "  LOW: " + Week.Kline.Low + "  OPEN: " + Week.Kline.Open + "  CLOSE: " + Week.Kline.Close); Console.WriteLine();
            //                    Console.WriteLine("MONTH:   " + "HIGH: " + Month.Kline.High + "  LOW: " + Month.Kline.Low + "  OPEN: " + Month.Kline.Open + "  CLOSE: " + Month.Kline.Close); Console.WriteLine();
            //                    Console.WriteLine("      *** ETH-USDT Candle *** "); Console.WriteLine();
            //                    Console.WriteLine("MINUTE:  " + "HIGH: " + MinuteETH.Kline.High + "  LOW: " + MinuteETH.Kline.Low + "  OPEN: " + MinuteETH.Kline.Open + "  CLOSE: " + MinuteETH.Kline.Close); Console.WriteLine();
            //                    Console.WriteLine("HOUR:    " + "HIGH: " + HourETH.Kline.High + "  LOW: " + HourETH.Kline.Low + "  OPEN: " + HourETH.Kline.Open + "  CLOSE: " + HourETH.Kline.Close); Console.WriteLine();
            //                    Console.WriteLine("DAY:     " + "HIGH: " + DayETH.Kline.High + "  LOW: " + DayETH.Kline.Low + "  OPEN: " + DayETH.Kline.Open + "  CLOSE: " + DayETH.Kline.Close); Console.WriteLine();
            //                    Console.WriteLine("WEEK:    " + "HIGH: " + WeekETH.Kline.High + "  LOW: " + WeekETH.Kline.Low + "  OPEN: " + WeekETH.Kline.Open + "  CLOSE: " + WeekETH.Kline.Close); Console.WriteLine();
            //                    Console.WriteLine("MONTH:   " + "HIGH: " + MonthETH.Kline.High + "  LOW: " + MonthETH.Kline.Low + "  OPEN: " + MonthETH.Kline.Open + "  CLOSE: " + MonthETH.Kline.Close); Console.WriteLine();
            //                    Console.WriteLine("      *** LTC-USDT Candle *** "); Console.WriteLine();
            //                    Console.WriteLine("MINUTE:  " + "HIGH: " + MinuteLTC.Kline.High + "  LOW: " + MinuteLTC.Kline.Low + "  OPEN: " + MinuteLTC.Kline.Open + "  CLOSE: " + MinuteLTC.Kline.Close); Console.WriteLine();
            //                    Console.WriteLine("HOUR:    " + "HIGH: " + HourLTC.Kline.High + "  LOW: " + HourLTC.Kline.Low + "  OPEN: " + HourLTC.Kline.Open + "  CLOSE: " + HourLTC.Kline.Close); Console.WriteLine();
            //                    Console.WriteLine("DAY:     " + "HIGH: " + DayLTC.Kline.High + "  LOW: " + DayLTC.Kline.Low + "  OPEN: " + DayLTC.Kline.Open + "  CLOSE: " + DayLTC.Kline.Close); Console.WriteLine();
            //                    Console.WriteLine("WEEK:    " + "HIGH: " + WeekLTC.Kline.High + "  LOW: " + WeekLTC.Kline.Low + "  OPEN: " + WeekLTC.Kline.Open + "  CLOSE: " + WeekLTC.Kline.Close); Console.WriteLine();
            //                    Console.WriteLine("MONTH:   " + "HIGH: " + MonthLTC.Kline.High + "  LOW: " + MonthLTC.Kline.Low + "  OPEN: " + MonthLTC.Kline.Open + "  CLOSE: " + MonthLTC.Kline.Close); Console.WriteLine();
            //                    Thread.Sleep(800);
            //                }
            //                Console.Clear();
            //            }
        }
    }
}
