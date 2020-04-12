using Exchange.Common;
using Exchange.DTO;
using Exchange.EF;
using Exchange.UOW;
using Ninject;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace OrderManagment
{
    public class OrderManagmentScript
    {
        private static IUnitOfWork uow  = null;
        private static List<Pair> pairs = new List<Pair>();
        private static List<Trade> tradeList = new List<Trade>();

        static void Main(string[] args)
        {
            try
            {
                IKernel kernel = new StandardKernel();
                UOWRegistration.BindAll(kernel);
                uow = kernel.Get<IUnitOfWork>();
            }
            catch (Exception ex) { Console.WriteLine("Error on Application Start:   " + ex.Message); }

            Console.WriteLine("****Order Management Script****");

            /////////////////////// OrderManagment //////////////
            Thread OrderManagment = new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        var pairs = uow.MarketRates.GetAllPairs();
                        if (pairs != null)
                        {
                            for (var k = 0; k < pairs.Count; k++)
                            {
                                tradeList.Clear();
                                tradeList = uow.Payment.GetOrders().Where(tr => tr.Symbol == pairs[k].MarketSymbol).ToList();
                                if (tradeList.Count != 0)
                                {
                                    //var ticker = uow.MarketRates.GetSpecificTickerBinance(UtilityMethods.convertSymbol(pairs[k].MarketSymbol));
                                    var candle = uow.MarketRates.GetCandleWebSocket(pairs[k].MarketSymbol, "1m");
                                    GetSpecificTickerBinance ticker = new GetSpecificTickerBinance() {
                                        symbol   = candle.SymbolName,
                                        bidPrice = candle.C.ToString(),
                                        askPrice = candle.C.ToString(),
                                        askQty   = "",
                                        bidQty   = "",
                                    };

                                    for (var i = 0; i < tradeList.Count; i++)
                                    {
                                        if (tradeList[i].Status.Equals("PENDING"))
                                        {
                                            var firstSym  = tradeList[i].Symbol.Split('-')[0];
                                            var secondSym = tradeList[i].Symbol.Split('-')[1];
                                            var tId = tradeList[i].TradeId;

                                            decimal PnLFinal = new Decimal();
                                            if (tradeList[i].Symbol == "BTC-USDT")
                                            {
                                                PnLFinal = 10;
                                            }
                                            else if (tradeList[i].Symbol == "ETH-USDT")
                                            {
                                                PnLFinal = 100;
                                            }
                                            else if (tradeList[i].Symbol == "LTC-USDT")
                                            {
                                                PnLFinal = 1000;
                                            }
                                            else if (tradeList[i].Symbol == "XRP-USDT")
                                            {
                                                PnLFinal = 100000;
                                            }
                                            else
                                            {
                                                PnLFinal = 10;
                                            }

                                            using (var db = new ExchangeEntities())
                                            {
                                                if (tradeList[i].StopLoss_TakeProfitEn.HasValue)
                                                {
                                                    if (tradeList[i].StopLoss_TakeProfitEn.Value)
                                                    {
                                                        List<Wallet> dbWallet = null;
                                                        string status = "";
                                                        var accId = tradeList[i].Account_Id;

                                                        if (tradeList[i].Direction == "BUY")
                                                        {
                                                            if (tradeList[i].UpLimitValue.Value <= decimal.Parse(ticker.askPrice) || tradeList[i].DownLimitValue.Value >= decimal.Parse(ticker.askPrice))
                                                            {
                                                                dbWallet = db.Wallets.Where(m => m.Account_Id == accId && m.Currency == "USD").ToList();

                                                                if (tradeList[i].PnL != null)
                                                                {
                                                                    if (tradeList[i].PnL >= 0)
                                                                    {
                                                                        dbWallet.First().Balance += Math.Abs(tradeList[i].PnL.Value);
                                                                    }
                                                                    else
                                                                    {
                                                                        dbWallet.First().Balance -= Math.Abs(tradeList[i].PnL.Value);
                                                                    }
                                                                    status = "COMPLETED";
                                                                }
                                                                else
                                                                {
                                                                    dbWallet.First().Balance += (tradeList[i].Amount * tradeList[i].Value);
                                                                    status = "EXPIRED";
                                                                }
                                                                var tradeId = tradeList[i].TradeId;
                                                                var tr = db.Trades.Where(x => x.TradeId == tradeId).FirstOrDefault();
                                                                tr.Status = status;
                                                                tr.TradeClose_Date = DateTime.UtcNow;
                                                                db.Configuration.ValidateOnSaveEnabled = false;
                                                                db.SaveChanges();
                                                                Console.WriteLine("Order with Account Id: " + accId + " and Order Id:  " + tradeList[i].TradeId + " is Closed");
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (tradeList[i].UpLimitValue.Value >= decimal.Parse(ticker.bidPrice) || tradeList[i].DownLimitValue.Value <= decimal.Parse(ticker.bidPrice))
                                                            {
                                                                dbWallet = db.Wallets.Where(m => m.Account_Id == accId && m.Currency == "USD").ToList();

                                                                if (tradeList[i].PnL != null)
                                                                {
                                                                    if (tradeList[i].PnL <= 0)
                                                                    {
                                                                        dbWallet.First().Balance += Math.Abs(tradeList[i].PnL.Value);
                                                                    }
                                                                    else
                                                                    {
                                                                        dbWallet.First().Balance -= Math.Abs(tradeList[i].PnL.Value);
                                                                    }
                                                                    status = "COMPLETED";
                                                                }
                                                                else
                                                                {
                                                                    dbWallet.First().Balance += (tradeList[i].Amount * tradeList[i].Value);
                                                                    status = "EXPIRED";
                                                                }
                                                                var tradeId = tradeList[i].TradeId;
                                                                var tr = db.Trades.Where(x => x.TradeId == tradeId).FirstOrDefault();
                                                                tr.Status = status;
                                                                tr.TradeClose_Date = DateTime.UtcNow;
                                                                db.Configuration.ValidateOnSaveEnabled = false;
                                                                db.SaveChanges();
                                                                Console.WriteLine("Order with Account Id: " + accId + " and Order Id:  " + tradeList[i].TradeId + " is Closed  :  Closed due to S/T ");
                                                            }
                                                        }
                                                    }
                                                }

                                                if (tradeList[i].Status == "PENDING")
                                                {
                                                    if (tradeList[i].expiryTime != -1)
                                                    {
                                                        double time = (double)(tradeList[i].expiryTime / 60);
                                                        DateTime tradeDate = tradeList[i].TradeDate.AddMinutes(time);
                                                        DateTime currentTime = DateTime.UtcNow;
                                                        var val = DateTime.Compare(tradeDate, currentTime);

                                                        var accId = tradeList[i].Account_Id;

                                                        if (val <= 0)   ////////////////   if Time expire   //////////////////
                                                        {
                                                            List<Wallet> dbWallet = null;
                                                            string status = "";
                                                            if (tradeList[i].Direction == "BUY")
                                                            {
                                                                dbWallet = db.Wallets.Where(m => m.Account_Id == accId && m.Currency == "USD").ToList();

                                                                if (tradeList[i].PnL != null)
                                                                {
                                                                    if (tradeList[i].PnL >= 0)
                                                                    {
                                                                        dbWallet.First().Balance += (Math.Abs(tradeList[i].PnL.Value));
                                                                    }
                                                                    else
                                                                    {
                                                                        dbWallet.First().Balance -= (Math.Abs(tradeList[i].PnL.Value));
                                                                    }
                                                                    status = "COMPLETED";
                                                                }
                                                                else
                                                                {
                                                                    dbWallet.First().Balance += (tradeList[i].Amount * tradeList[i].Value);
                                                                    status = "EXPIRED";
                                                                }
                                                            }
                                                            else
                                                            {
                                                                dbWallet = db.Wallets.Where(m => m.Account_Id == accId && m.Currency == "USD").ToList();

                                                                if (tradeList[i].PnL != null)
                                                                {
                                                                    if (tradeList[i].PnL <= 0)
                                                                    {
                                                                        dbWallet.First().Balance += (Math.Abs(tradeList[i].PnL.Value));
                                                                    }
                                                                    else
                                                                    {
                                                                        dbWallet.First().Balance -= (Math.Abs(tradeList[i].PnL.Value));
                                                                    }
                                                                    status = "COMPLETED";
                                                                }
                                                                else
                                                                {
                                                                    dbWallet.First().Balance += (tradeList[i].Amount * tradeList[i].Value);
                                                                    status = "EXPIRED";
                                                                }
                                                            }
                                                            var tradeId = tradeList[i].TradeId;
                                                            var tr = db.Trades.Where(x => x.TradeId == tradeId).FirstOrDefault();
                                                            tr.Status = status;
                                                            tr.TradeClose_Date = DateTime.UtcNow;
                                                            db.Configuration.ValidateOnSaveEnabled = false;
                                                            db.SaveChanges();
                                                            Console.WriteLine("Order with Account Id: " + accId + " and order Id:  " + tradeList[i].TradeId + " is Closed");
                                                        }

                                                        else  // if there is some time remaining
                                                        {
                                                            if (tradeList[i].Direction.Equals("BUY"))
                                                            {
                                                                var tm = db.Trades.Where(t => t.TradeId == tId).FirstOrDefault();
                                                                if (tm.Status == "PENDING")
                                                                {
                                                                    tm.PnL = (decimal.Parse(ticker.askPrice) - tm.Rate) * PnLFinal * tm.Amount;
                                                                    tm.ExitPrice = decimal.Parse(ticker.askPrice);
                                                                    db.Configuration.ValidateOnSaveEnabled = false;
                                                                    db.SaveChanges();
                                                                }
                                                            }
                                                            else if (tradeList[i].Direction.Equals("SELL"))
                                                            {
                                                                var tm = db.Trades.Where(t => t.TradeId == tId).FirstOrDefault();
                                                                if (tm.Status == "PENDING")
                                                                {
                                                                    tm.PnL = (decimal.Parse(ticker.bidPrice) - tm.Rate) * PnLFinal * tm.Amount;
                                                                    tm.ExitPrice = decimal.Parse(ticker.bidPrice);
                                                                    db.Configuration.ValidateOnSaveEnabled = false;
                                                                    db.SaveChanges();
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (tradeList[i].Direction.Equals("BUY"))
                                                        {
                                                            var tm = db.Trades.Where(t => t.TradeId == tId).FirstOrDefault();
                                                            if (tm.Status == "PENDING")
                                                            {
                                                                tm.PnL = (decimal.Parse(ticker.askPrice) - tm.Rate) * PnLFinal * tm.Amount;
                                                                tm.ExitPrice = decimal.Parse(ticker.askPrice);
                                                                db.Configuration.ValidateOnSaveEnabled = false;
                                                                db.SaveChanges();
                                                            }
                                                        }
                                                        else if (tradeList[i].Direction.Equals("SELL"))
                                                        {
                                                            var tm = db.Trades.Where(t => t.TradeId == tId).FirstOrDefault();
                                                            if (tm.Status == "PENDING")
                                                            {
                                                                tm.PnL = (decimal.Parse(ticker.bidPrice) - tm.Rate) * PnLFinal * tm.Amount;
                                                                tm.ExitPrice = decimal.Parse(ticker.bidPrice);
                                                                db.Configuration.ValidateOnSaveEnabled = false;
                                                                db.SaveChanges();
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        var LineNumber = new StackTrace(ex, true).GetFrame(0).GetFileLineNumber();
                        Console.WriteLine(ex.Message.ToString() + " -------- Line No:  " + LineNumber);
                    }
                    Thread.Sleep(1000);
                }
            });
            OrderManagment.Start();

            ////////////////////////////////// User Balance Checking ////////////////////
            Thread UserBalance = new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        List<Account> users = uow.Accounts.GetAllUsers();
                        foreach (var user in users)
                        {
                            List<Trade> trades = uow.Payment.GetOrdersById(user.AccountId);
                            if (trades.Count != 0)
                            {
                                CalculatePnL(trades, user);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        var LineNumber = new StackTrace(ex, true).GetFrame(0).GetFileLineNumber();
                        Console.WriteLine(ex.Message.ToString() + " -------- Line No:  " + LineNumber);
                    }
                    Thread.Sleep(8000);
                }
            });
            UserBalance.Start();
        }

        private static void CalculatePnL(List<Trade> trades, Account user)
        {
            using (var db = new ExchangeEntities())
            {
                var dbWallet = db.Wallets.Where(m => m.Account_Id == user.AccountId && m.Currency == "USD").ToList();
                if (dbWallet != null)
                {
                    decimal dummyPnL = 0;
                    foreach (var trade in trades)
                    {
                        var firstSym  = trade.Symbol.Split('-')[0];
                        var secondSym = trade.Symbol.Split('-')[1];
                        var tId = trade.TradeId;

                        if (trade.Direction == "BUY")
                        {
                            if (trade.PnL != null)
                            {
                                if (trade.PnL >= 0)
                                {
                                    dummyPnL += Math.Abs(trade.PnL.Value);
                                }
                                else
                                {
                                    dummyPnL -= Math.Abs(trade.PnL.Value);
                                }
                            }
                        }
                        else
                        {
                            if (trade.PnL != null)
                            {
                                if (trade.PnL <= 0)
                                {
                                    dummyPnL += Math.Abs(trade.PnL.Value);
                                }
                                else
                                {
                                    dummyPnL -= Math.Abs(trade.PnL.Value);
                                }
                            }
                        }
                    }

                    if ((dummyPnL + dbWallet.FirstOrDefault().Balance) <= 10)
                    {
                        foreach (var obj in trades)
                        {
                            string status = "";
                            if (obj.Direction == "BUY")
                            {
                                if (obj.PnL != null)
                                {
                                    if (obj.PnL >= 0)
                                    {
                                        dbWallet.First().Balance += (Math.Abs(obj.PnL.Value));
                                    }
                                    else
                                    {
                                        dbWallet.First().Balance -= (Math.Abs(obj.PnL.Value));
                                    }
                                    status = "COMPLETED";
                                }
                                else
                                {
                                    dbWallet.First().Balance += (obj.Amount * obj.Value);
                                    status = "EXPIRED";
                                }
                            }
                            else
                            {
                                if (obj.PnL != null)
                                {
                                    if (obj.PnL <= 0)
                                    {
                                        dbWallet.First().Balance += (Math.Abs(obj.PnL.Value));
                                    }
                                    else
                                    {
                                        dbWallet.First().Balance -= (Math.Abs(obj.PnL.Value));
                                    }
                                    status = "COMPLETED";
                                }
                                else
                                {
                                    dbWallet.First().Balance += (obj.Amount * obj.Value);
                                    status = "EXPIRED";
                                }
                            }
                            var tm = db.Trades.Where(t => t.TradeId == obj.TradeId).FirstOrDefault();
                            tm.Status = status;
                            tm.TradeClose_Date = DateTime.UtcNow;
                            db.Configuration.ValidateOnSaveEnabled = false;
                            db.SaveChanges();
                            Console.WriteLine("Order with Account Id:  " + user.AccountId + " and Order Id:  " + obj.TradeId + " Order Pair:  " + obj.Symbol + "  is Closed.  ---  ForceFully");
                        }
                    }
                }
            }

        }

    }
}




//decimal PnLFinal = new Decimal();
//if (tradeList[i].Symbol == "BTC-USDT")
//{
//    PnLFinal = (10 * Math.Abs(tradeList[i].PnL.Value));
//}
//else if (tradeList[i].Symbol == "ETH-USDT")
//{
//    PnLFinal = (100 * Math.Abs(tradeList[i].PnL.Value));
//}
//else if (tradeList[i].Symbol == "LTC-USDT")
//{
//    PnLFinal = (1000 * Math.Abs(tradeList[i].PnL.Value));
//}
//else if (tradeList[i].Symbol == "XRP-USDT")
//{
//    PnLFinal = (100000 * Math.Abs(tradeList[i].PnL.Value));
//}
//else
//{
//    PnLFinal = (10 * Math.Abs(tradeList[i].PnL.Value));
//}


////////////////////////////////// Syncing OrderBook ////////////////////
//Thread SyncOrderBook = new Thread(() =>
//{
//    while (true)
//    {
//        try
//        {
//            pairs     = uow.MarketRates.GetAllPairs();
//            orderBook = new Dictionary<string, OrderBookModel>();
//            for (var z = 0; z < pairs.Count; z++)
//            {
//                OrderBookModel temp = uow.MarketRates.GetOrderBook(pairs[z].MarketSymbol);
//                if (temp != null)
//                {
//                    orderBook.Add(pairs[z].MarketSymbol, temp);
//                }
//                else
//                {
//                    orderBook.Add(pairs[z].MarketSymbol, new OrderBookModel());
//                }
//            }
//        }
//        catch (Exception) { }
//        Thread.Sleep(2000);
//    }
//});
//SyncOrderBook.Start();
