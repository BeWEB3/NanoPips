using Exchange.Common;
using Exchange.DTO;
using Exchange.UI.Models;
using Exchange.UOW;
using Newtonsoft.Json;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Exchange.UI.Controllers
{
    [NoCache]
    [ExchangeAuthorize]
    public class DashboardController : Controller
    {
        private IUnitOfWork _uow;

        public DashboardController(IUnitOfWork uow) { _uow = uow; }

        public ActionResult Index(string symbol = "BTC-USDT", string timeFrame = "1m", decimal utcTime = 0, string referral = "")
        {
            var acc = SessionItems.Get(SessionKey.ACCOUNT) as Account;
            var orderBook = _uow.MarketRates.GetOrderBookBinance(UtilityMethods.convertSymbol(symbol));
            IndexModel IndexModel;
            if (acc == null)
            {
                IndexModel = new IndexModel()
                {
                    AcceptTermConditions = true,
                    GetPairs = _uow.Accounts.GetPairs(),
                    MarketName = symbol,
                    GetTradeHistory = new List<Trade>(),
                    GetCandles = ConvertCandles(symbol, timeFrame, utcTime),
                    GetOrderBook = orderBook,
                    TimeFrameValue = timeFrame,
                    timeOffset = utcTime,
                    UserRoles = new UserRole(),
                    IsLogin = false,
                    referral = referral,
                };
            }
            else
            {
                IndexModel = new IndexModel()
                {
                    AcceptTermConditions = (acc.IsAgreeTermServices == null) ? false : acc.IsAgreeTermServices.Value,
                    GetPairs = _uow.Accounts.GetPairs(),
                    MarketName = symbol,
                    GetTradeHistory = _uow.Accounts.GetTradeHistory(acc.AccountId),
                    GetCandles = ConvertCandles(symbol, timeFrame, utcTime),
                    GetOrderBook = orderBook,
                    TimeFrameValue = timeFrame,
                    timeOffset = utcTime,
                    UserRoles = new UserRole(),
                    IsLogin = true,
                };
            }
            return View(IndexModel);
        }

        [HttpPost]
        public string ForgotPassword(IndexModel indexModel)
        {
            try
            {
                _uow.Accounts.SendForgotPasswordEmail(indexModel.UserRoles.Username.Trim().ToLower());
                return "<div class='col-md-12'>" +
                        "<b style='font-size:18px; font-weight:500;'>An email with password reset instructions was sent to your email address.</b>" +
                        " <br /><br /> <span  style='font-weight:bold;color:green; font-size:20px;'> <b> Remember: <b/></span> <span style='font-size:16px; font-weight:500;'>Check your Spam Folder</span> </span> </div>";
            }
            catch (ExchangeException ex)
            {
                return ex.ErrorMessage;
            }
        }

        public ActionResult Light()
        {
            Session["DARK"] = null;
            return Redirect(Request.UrlReferrer.LocalPath);
        }

        public ActionResult Dark()
        {
            Session["DARK"] = true;
            return Redirect(Request.UrlReferrer.LocalPath);
        }

        [HttpPost]
        public ActionResult Login(IndexModel model, double utcTime = 0)
        {
            try
            {
                var ac = _uow.Accounts.Login(model.UserRoles.Username, model.UserRoles.Password);
                if (ac.AccountEnabled == false)
                {
                    ViewBag.error = "Your account is disabled, please contact support for further assistance";
                    return View();
                }
                if (ac.AccountType_Id == (int)AccountTypes.ADMIN)
                {
                    ViewBag.error = "Invalid Login";
                    return View();
                }
                SessionItems.Add(SessionKey.ACCOUNT, ac);
                _uow.Accounts.SaveUserActivity(Request.UserAgent, Request.UserHostAddress, ac.Email, "Login");
                return RedirectToAction("Index", "Dashboard", new { utcTime });
            }
            catch (ExchangeException ex)
            {
                TempData["errorLoading"] = ex.ErrorMessage;
                return RedirectToAction("Index", "Dashboard", new { utcTime });
            }

        }

        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public string SignUp(IndexModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //var ac = _uow.Accounts.SignUp(model.UserRoles.Username.ToLower().Trim(), model.UserRoles.Password', model.UserRoles.RefferenceNumber);
                    var ac = _uow.Accounts.SignUp(model.UserRoles.Username.ToLower().Trim(), model.UserRoles.Password, model.UserRoles.RefferenceNumber);
                    _uow.Accounts.SendVerificationEmail(ac.AccountId);
                    return "<div class='col-md-12' style='text-align:center'>" +
                        "<b style='text-align:center; font-size:25px; font-weight:bold;'>Verify</b>" +
                        " <br /><br /><span style='font-size:16px; font-weight:500;'>Access the sent to your Email</span> <br/> <span style='font-size:16px; font-weight:500;'>Click the link sent to your email to Activate your account.  </span> " +
                        " <br /><br /> <span  style='font-weight:bold;color:green; font-size:23px;'> <b> Remember: <b/></span> <span style='font-size:16px; font-weight:500;'>Check your Spam Folder</span> </span> </div>";
                }
                catch (ExchangeException ex)
                {
                    return ex.Message.Replace("_", " ").ToLower();
                }
            }
            else return "Please fill the required fields first";
        }

        public ActionResult GetDepositDetail(string cur)
        {
            var address = _uow.Payment.GetDepositAddress(cur);
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(address, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            var bytes = BitmapToBytes(qrCodeImage);
            var barCode = Convert.ToBase64String(bytes);
            var obj = new
            {
                address,
                cur,
                barCode
            };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAddressList(string cur)
        {
            var acc = SessionItems.Get(SessionKey.ACCOUNT) as Account;
            List<AddressBook> addressList = _uow.Payment.GetCurrencyAddressBook(cur, acc.AccountId);
            var addresses = addressList.Select(x => x.Address).ToList();
            List<int> addressId = new List<int>() { 0, 0, 0, 0, 0 };
            var temp = addressList.Select(x => x.AdressId).ToList();
            for (var i = 0; i < temp.Count; i++)
            {
                addressId[i] = temp[i];
            }
            return Json(new { addresses, addressId, currency = cur, accountId = acc.AccountId }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddAddress(string cur, string address, long addressid)
        {
            var acc = SessionItems.Get(SessionKey.ACCOUNT) as Account;
            List<AddressBook> addressList = _uow.Payment.AddWhitListAddress(cur, acc.AccountId, addressid, address);
            if (addressList.Count == 0)
            {
                return Json(new { status = false }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { status = true }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DeleteAddress(string cur, string address, long addressid)
        {
            var acc = SessionItems.Get(SessionKey.ACCOUNT) as Account;
            var stat = _uow.Payment.DeleteWhitListAddress(cur, acc.AccountId, addressid, address);
            return Json(new { status = stat }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddressAuthorize(string cur)
        {
            var acc = SessionItems.Get(SessionKey.ACCOUNT) as Account;
            var stat = _uow.Payment.AddressAuthorize(cur, acc.AccountId, acc.Email);
            return Json(new { status = stat }, JsonRequestBehavior.AllowGet);
        }

        private static byte[] BitmapToBytes(Bitmap img)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }

        public ActionResult CryptoWithDrawl(string address, string symbol, double amount)
        {
            var _ac = SessionItems.Get(SessionKey.ACCOUNT) as Account;
            var ac = _uow.Accounts.GetAccount(_ac.AccountId);
            try
            {
                var a = _uow.Payment.WithDrawlReqAdmin(
                new Payment() { Account_Id = ac.AccountId, Amount = (decimal)amount, Currency = symbol, ToWalletAddress = address });
                SessionItems.Add(SessionKey.ACCOUNT, a);
                TempData["msg"] = "Withdraw request has been successfully sent to admin. Wait for the approval.";
                var obj = new
                {
                    status = true,
                    message = TempData["msg"]
                };
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            catch (ExchangeException ex)
            {
                var obj = new
                {
                    status = false,
                    message = ex.ErrorMessage.ToString()
                };
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public string Sell(string pair, decimal rate, decimal amount, int type, int expirytime, decimal timeOffset)
        {
            try
            {
                var ac = SessionItems.Get(SessionKey.ACCOUNT) as Account;
                if (rate == -1)
                {
                    //var ticker = _uow.MarketRates.GetSpecificTicker(pair);
                    var ticker = _uow.MarketRates.GetSpecificTickerBinance(UtilityMethods.convertSymbol(pair));
                    if (type == 2)
                    {
                        rate = decimal.Parse(ticker.bidPrice);
                    }
                }
                var response = _uow.Payment.Sell(ac.AccountId, pair, rate, amount, type, expirytime, timeOffset);
                return response;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpPost]
        public bool UpdateST(long tradeId, decimal stopLoss, decimal takeProfit)
        {
            try
            {
                return _uow.Payment.UpdateST(tradeId, stopLoss, takeProfit);
            }
            catch (Exception ex) {
                return false;
            }
        }

        [HttpPost]
        public string Buy(string pair, decimal rate, decimal amount, int type, int expirytime, decimal timeOffset)
        {
            // 1 for market order , 2 for limit order///
            try
            {
                var ac = SessionItems.Get(SessionKey.ACCOUNT) as Account;
                if (rate == -1)
                {
                    var ticker = _uow.MarketRates.GetSpecificTickerBinance(UtilityMethods.convertSymbol(pair));
                    if (type == 2)
                    {
                        rate = decimal.Parse(ticker.askPrice);
                    }
                }
                var response = _uow.Payment.Buy(ac.AccountId, pair, rate, amount, type, expirytime, timeOffset);
                return response;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public ActionResult Chart()
        {
            return View();
        }

        public ActionResult GetPendingOrders()
        {
            var ac = SessionItems.Get(SessionKey.ACCOUNT) as Account;
            if (ac == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                var res = _uow.Accounts.GetPendingTrades(ac.AccountId);
                List<PendingOrderForView> list = new List<PendingOrderForView>();
                for (var t = 0; t < res.Count; t++)
                {
                    long rTime = 0;
                    if (res[t].expiryTime == -1)
                    {
                        rTime = -1;
                    }
                    else
                    {
                        DateTime current = DateTime.UtcNow;
                        DateTime addedTime = res[t].TradeDate.AddMinutes((double)res[t].expiryTime / 60);
                        long remaining = (long)(addedTime - current).TotalSeconds;
                        rTime = (remaining >= 0) ? remaining : 0;
                    }

                    list.Add(new PendingOrderForView()
                    {
                        TradeId = res[t].TradeId,
                        TradeDate = res[t].TradeDate.ToString(),
                        TradeType = res[t].TradeType,
                        TradeTypeValue = res[t].TradeTypeValue,
                        Currency = res[t].Currency,
                        Symbol = res[t].Symbol,
                        Amount = (decimal)res[t].Amount,
                        Rate = (decimal)res[t].Rate,
                        Status = res[t].Status,
                        Account_Id = (long)res[t].Account_Id,
                        Value = (decimal)res[t].Value,
                        Direction = res[t].Direction,
                        expiryTime = rTime,
                        ExitPrice = res[t].ExitPrice,
                        PnL = res[t].PnL,
                        UpPrice = res[t].UpLimitValue.Value,
                        DownPrice = res[t].DownLimitValue.Value,
                        ST_Enable = res[t].StopLoss_TakeProfitEn.Value,
                    });
                }

                var pendingOrders = new
                {
                    orders = list
                };

                return Json(new { pendingOrders }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult UpdateBalance()
        {
            try
            {
                var ac = SessionItems.Get(SessionKey.ACCOUNT) as Account;
                if (ac == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    var res = _uow.Payment.GetWallets(ac.AccountId, "USD");

                    var balance = new
                    {
                        bal = res.FirstOrDefault().Balance
                    };

                    return Json(new { balance }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {

                var balance = new
                {
                    bal = 0
                };

                return Json(new { balance }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult GetOrderBook(string pair)
        {
            var ac = SessionItems.Get(SessionKey.ACCOUNT) as Account;
            GetOrderBookBinance res = _uow.MarketRates.GetOrderBookBinance(UtilityMethods.convertSymbol(pair));
            var orderBook = new
            {
                book = res
            };
            return Json(new { orderBook }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCandlesData(string pair, string timeFrame)
        {
            //var ac = SessionItems.Get(SessionKey.ACCOUNT) as Account;
            //if (ac == null)
            //{
            //    return RedirectToAction("Login", "Account");
            //}
            //else
            //{
            List<List<decimal?>> hloc = new List<List<decimal?>>();
            List<long?> v1 = new List<long?>();
            List<long?> xSeries = new List<long?>();
            var candles = (object[][])_uow.MarketRates.GetTestCandles(UtilityMethods.convertSymbol(pair), timeFrame, 1);

            var tmp = candles[candles.Length - 1];

            object obj = new
            {
                High = decimal.Parse(tmp[2].ToString()),
                Low = decimal.Parse(tmp[3].ToString()),
                Open = decimal.Parse(tmp[1].ToString()),
                Close = decimal.Parse(tmp[4].ToString()),
                Volume = decimal.Parse(tmp[5].ToString()),
                Ticker = ((long)tmp[0] / 1000),
            };
            return Json(new { obj }, JsonRequestBehavior.AllowGet);
            //}
        }

        public ActionResult GetTradeHistorySpecific(string dateFrom, string dateTo)
        {
            var ac = SessionItems.Get(SessionKey.ACCOUNT) as Account;
            if (ac == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                DateTime from, to;
                if (dateFrom != "" || dateTo != "")
                {
                    var tempdateFromTemp = dateFrom.Split('/');
                    dateFrom = tempdateFromTemp[0] + "-" + tempdateFromTemp[1] + "-" + tempdateFromTemp[2];
                    var tempdateToTemp = dateTo.Split('/');
                    dateTo = tempdateToTemp[0] + "-" + tempdateToTemp[1] + "-" + tempdateToTemp[2];
                    from = DateTime.Parse(dateFrom).ToUniversalTime();
                    to = DateTime.Parse(dateTo).ToUniversalTime();
                }
                else
                {
                    from = DateTime.UtcNow.AddYears(-20);
                    to = DateTime.UtcNow;
                }
                var paymentsList = _uow.Payment.GetSpecificPayments(ac.AccountId, from, to);
                var res = _uow.Accounts.GetTradeHistory(ac.AccountId, from, to);
                List<PendingOrderForView> list = new List<PendingOrderForView>();
                for (int t = 0; t < res.Count; t++)
                {
                    list.Add(new PendingOrderForView()
                    {
                        TradeId = res[t].TradeId,
                        TradeDate = res[t].TradeDate.ToString(),
                        TradeType = res[t].TradeType,
                        TradeTypeValue = res[t].TradeTypeValue,
                        Currency = res[t].Currency,
                        Symbol = res[t].Symbol,
                        Amount = (decimal)res[t].Amount,
                        Rate = (decimal)res[t].Rate,
                        Status = res[t].Status,
                        ExitPrice = res[t].ExitPrice,
                        Account_Id = (long)res[t].Account_Id,
                        Value = (decimal)res[t].Value,
                        Direction = res[t].Direction,
                        PnL = res[t].PnL,
                        Ticker = (res[t].TradeDate.Ticks - 621355968000000000) / 10000000,
                        isTradeOrder = true,
                    });
                }

                for (int t = 0; t < paymentsList.Count; t++)
                {
                    list.Add(new PendingOrderForView()
                    {
                        TradeId = paymentsList[t].PaymentId,
                        TradeDate = paymentsList[t].PaymentDate.ToString(),
                        TradeType = paymentsList[t].PaymentType,
                        TradeTypeValue = 0,
                        Currency = paymentsList[t].Currency,
                        Symbol = paymentsList[t].PaymentType,
                        Amount = (decimal)paymentsList[t].Amount,
                        Rate = 0,
                        Status = PaymentStatusType.COMPLETED.ToString(),
                        ExitPrice = 0,
                        Account_Id = (long)paymentsList[t].Account_Id,
                        Value = 0,
                        Direction = paymentsList[t].PaymentType,
                        PnL = (decimal)paymentsList[t].Amount,
                        isTradeOrder = false,
                        Ticker = (paymentsList[t].PaymentDate.Value.Ticks - 621355968000000000) / 10000000,
                    });
                }

                list = list.OrderByDescending(o => o.Ticker).ToList();
                return Json(new { list }, JsonRequestBehavior.AllowGet);
            }
        }

        public string ConvertCandles(string symbol, string timeFrame, decimal utcTime)
        {
            List<GetCandlesResponse> candles = new List<GetCandlesResponse>();
            List<List<decimal?>> hloc = new List<List<decimal?>>();
            List<long?> v1 = new List<long?>();
            List<long?> xSeries = new List<long?>();
            //var candles               = _uow.MarketRates.GetCandles(symbol, TimeFrame);
            var candlesBinance = (object[][])_uow.MarketRates.GetTestCandles(UtilityMethods.convertSymbol(symbol), timeFrame, 700);

            for (var val = 0; val < candlesBinance.Length; val++)
            {
                hloc.Add(new List<decimal?>
                {
                    decimal.Parse(candlesBinance[val][2].ToString()),
                    decimal.Parse(candlesBinance[val][3].ToString()),
                    decimal.Parse(candlesBinance[val][1].ToString()),
                    decimal.Parse(candlesBinance[val][4].ToString())
                });
                v1.Add((long)decimal.Parse(candlesBinance[val][9].ToString()));
                xSeries.Add((long)candlesBinance[val][0] / 1000);
                //xSeries.Add(((long)candlesBinance[val][0] - 621355968000000000) / 10000000);
            }

            //for (var x = 0; x < candles.Count; x++)
            //{
            //    hloc.Add(new List<decimal?>
            //    {
            //        candles[x].High,
            //        candles[x].Low,
            //        candles[x].Open,
            //        candles[x].Close
            //    });
            //    v1.Add((long)candles[x].BaseVolume);
            //    long epoch = ((candles[x].StartsAt).Ticks - 621355968000000000) / 10000000;
            //    xSeries.Add(epoch);
            //}

            var startTime = xSeries.Count - 25;
            var endTime = xSeries.Count - 1;

            DateTime mFromDateTime = UnixTimeStampToDateTime(xSeries[startTime].Value).AddMinutes((-1 * (double)(utcTime)));
            DateTime mToDateTime = UnixTimeStampToDateTime(xSeries[endTime].Value).AddMinutes(((-1 * (double)(utcTime))));

            ChartResponse chart = new ChartResponse()
            {
                data = new Data()
                {
                    HLOC = new HLOC() { LKOH = hloc },
                    V1 = new V1() { LKOH = v1 },
                    XSeries = new XSeries() { LKOH = xSeries }
                },
                dataSettings = new DataSetting()
                {
                    useHash = false,
                    dateFrom = mFromDateTime.Day.ToString("D2") + "." + mFromDateTime.Month.ToString("D2") + "." + mFromDateTime.Year.ToString("D4") + " " + mFromDateTime.Hour.ToString("D2") + ":" + mFromDateTime.Minute.ToString("D2") + ":" + mFromDateTime.Second.ToString("D2"),
                    dateTo = mToDateTime.Day.ToString("D2") + "." + mToDateTime.Month.ToString("D2") + "." + mToDateTime.Year.ToString("D4") + " " + mToDateTime.Hour.ToString("D2") + ":" + mToDateTime.Minute.ToString("D2") + ":" + mToDateTime.Second.ToString("D2"),
                    start = mFromDateTime.Day.ToString("D2") + "." + mFromDateTime.Month.ToString("D2") + "." + mFromDateTime.Year.ToString("D4") + " " + mFromDateTime.Hour.ToString("D2") + ":" + mFromDateTime.Minute.ToString("D2") + ":" + mFromDateTime.Second.ToString("D2"),
                    end = mToDateTime.Day.ToString("D2") + "." + mToDateTime.Month.ToString("D2") + "." + mToDateTime.Year.ToString("D4") + " " + mToDateTime.Hour.ToString("D2") + ":" + mToDateTime.Minute.ToString("D2") + ":" + mToDateTime.Second.ToString("D2"),
                    graphicIndicator = "",
                    hash = "",
                    id = "LKOH",
                    interval = "I15",
                    timeFrame = 1440
                }
            };

            var call = JsonConvert.SerializeObject(chart);
            var call2 = call.Remove((call.Length - 1), 1).Remove(0, 2);
            call2 = call2.Remove(4, 1);

            for (var i = 0; i < call2.Length; i++)
            {
                if (call2[i].Equals('d') && call2[i + 4].Equals('S'))
                {
                    call2 = call2.Remove((i - 1), 1).Remove((i + 11), 1);
                    break;
                }
            }
            return call2;
        }

        public static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToUniversalTime();
            return dtDateTime;
        }

        public ActionResult GetTradeOnChart(decimal timeOffset, string pair, long point)
        {
            var ac = SessionItems.Get(SessionKey.ACCOUNT) as Account;
            if (ac == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                List<GetTradeonChart> tradeonCharts = new List<GetTradeonChart>();
                var res = new object();
                if (ac != null)
                {
                    var list = _uow.Accounts.GetPendingTrades(ac.AccountId, pair);
                    for (int x = 0; x < list.Count; x++)
                    {
                        list[x].TradeDate = list[x].TradeDate.AddHours(-1 * (double)(timeOffset / 60));

                        string dt = (list[x].TradeDate.Year + "-" + list[x].TradeDate.Month.ToString("D2") + "-" + list[x].TradeDate.Day.ToString("D2") + " " + list[x].TradeDate.Hour.ToString("D2") +
                                                              ":" + list[x].TradeDate.Minute.ToString("D2") + ":" + list[x].TradeDate.Second.ToString("D2"));
                        tradeonCharts.Add(new GetTradeonChart()
                        {
                            id = list[x].TradeId,
                            volume = list[x].Amount.Value.ToString(),
                            price = list[x].Rate.ToString(),
                            splitPrice = list[x].Rate.Value,
                            profit = (list[x].PnL == null) ? "0" : list[x].PnL.Value.ToString(),
                            count = 1,
                            summ = list[x].Amount.Value.ToString(),
                            qb = "",
                            time = dt,
                            date_time = dt,
                            security_id = list[x].TradeId,
                            type_id = (list[x].Direction.Equals("BUY")) ? 1 : 2,
                        });
                    }

                    for (int x = 0; x < list.Count; x++)
                    {
                        list[x].TradeDate = UnixTimeStampToDateTime(point).AddHours(-1 * (double)(timeOffset / 60)).AddMinutes(60);

                        string dt = (list[x].TradeDate.Year + "-" + list[x].TradeDate.Month.ToString("D2") + "-" + list[x].TradeDate.Day.ToString("D2") + " " + list[x].TradeDate.Hour.ToString("D2") +
                                                              ":" + list[x].TradeDate.Minute.ToString("D2") + ":" + list[x].TradeDate.Second.ToString("D2"));
                        long upId = new Random().Next(1, 100000);
                        tradeonCharts.Add(new GetTradeonChart()
                        {
                            id = upId,
                            volume = list[x].Amount.Value.ToString(),
                            price = (list[x].UpLimitValue).ToString(),
                            splitPrice = list[x].UpLimitValue.Value,
                            profit = (list[x].PnL == null) ? "0" : list[x].PnL.Value.ToString(),
                            count = 1,
                            summ = list[x].Amount.Value.ToString(),
                            qb = "",
                            time = dt,
                            date_time = dt,
                            security_id = upId,
                            type_id = (list[x].Direction.Equals("BUY")) ? 1 : 2,
                        });
                        long downId = new Random().Next(100000, 100000000);
                        tradeonCharts.Add(new GetTradeonChart()
                        {
                            id = downId,
                            volume = list[x].Amount.Value.ToString(),
                            price = (list[x].DownLimitValue).ToString(),
                            splitPrice = list[x].DownLimitValue.Value,
                            profit = (list[x].PnL == null) ? "0" : list[x].PnL.Value.ToString(),
                            count = 1,
                            summ = list[x].Amount.Value.ToString(),
                            qb = "",
                            time = dt,
                            date_time = dt,
                            security_id = downId,
                            type_id = (list[x].Direction.Equals("BUY")) ? 1 : 2,
                        });
                    }
                    //res = JsonConvert.SerializeObject(tradeonCharts);
                    res = tradeonCharts;
                    return Json(new { res }, JsonRequestBehavior.AllowGet);
                }
                //res = JsonConvert.SerializeObject(tradeonCharts);
                res = tradeonCharts;
                return Json(new { res }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult CloseOrder(long orderId)
        {

            var ac = SessionItems.Get(SessionKey.ACCOUNT) as Account;
            if (ac == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var obj = _uow.Payment.CloseOrder(orderId, ac.AccountId);
            var msg = "";
            if (obj != null)
            {
                msg = "Order has been Successfully Close.";
                return Json(new { msg }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                msg = "Order not canceled, An Error Occured";
                return Json(new { msg }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SetAcceptTerms(bool value)
        {
            var ac = SessionItems.Get(SessionKey.ACCOUNT) as Account;
            if (ac == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var obj = _uow.Accounts.SetTerm_Conditions(ac.AccountId, value);
            bool res = false;
            if (obj != null)
            {
                SessionItems.Add(SessionKey.ACCOUNT, obj);
                res = true;
                return Json(new { res }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                res = false;
                return Json(new { res }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}


