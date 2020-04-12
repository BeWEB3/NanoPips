using Exchange.UOW;
using Ninject;
using System;
using WebSocket4Net;
using PolygonWeb;
using BinanceExchange.API.Websockets;
using BinanceExchange.API.Models.WebSocket;
using BinanceExchange.API.Enums;
using BinanceExchange.API.Client;
using Newtonsoft.Json;

namespace TestConsole
{
    public class Program
    {
        private static IUnitOfWork uow = null;
        private WebSocket websocket;

        static void Main(string[] args)
        {
            //try
            //{
            //    IKernel kernel = new StandardKernel();
            //    UOWRegistration.BindAll(kernel);
            //    uow = kernel.Get<IUnitOfWork>();
            //}
            //catch (Exception ex) { Console.WriteLine("Error on Application Start:   " + ex.Message); }

            //var obj = new PolygonWebConnect();
            //obj.Start();

            var client = new BinanceClient(new ClientConfiguration()
            {
                ApiKey = "9fc2M7WrNl6FdsEv9Pk80eGXy68bZgtUscp1oOPX6w2cnOKQyGdvrtoEAox9gQR2",
                SecretKey = "fS7RG3om3ChfRI2JpD5cV1Qj2ZWwIvG8ANtRlrluDqSRVmz6bzDgS3CR5T0qp2gf",
            });

            var manualWebSocketClient = new InstanceBinanceWebSocketClient(client);
            var socketId = manualWebSocketClient.ConnectToKlineWebSocket("BTCUSDT", KlineInterval.OneMinute, data =>
            {
                System.Console.WriteLine($"1 Minute:  {JsonConvert.SerializeObject(data)}");
            });

            var manualWebSocketClient2 = new InstanceBinanceWebSocketClient(client);
            var socketId2 = manualWebSocketClient2.ConnectToKlineWebSocket("BTCUSDT", KlineInterval.OneHour, data =>
            {
                System.Console.WriteLine($"1 Hour:  {JsonConvert.SerializeObject(data)}");
            });

            //manualWebSocketClient.CloseWebSocketInstance(socketId);

            Console.ReadKey();
        }
    }
    }







    //using (ExchangeEntities db = new ExchangeEntities())
    //{
    //    var accList = db.Accounts.ToList();
    //    foreach (var acc in accList) {
    //        var val = db.Accounts.Where(t => t.AccountId == acc.AccountId).FirstOrDefault();
    //        val.RefferenceNumber = val.Email.Substring(0, 3) + val.AccountId;
    //        db.Configuration.ValidateOnSaveEnabled = false;
    //        db.SaveChanges();
    //    }
    //}

