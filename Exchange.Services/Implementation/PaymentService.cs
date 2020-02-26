using Exchange.Common;
using Exchange.DTO;
using Exchange.EF;
using Exchange.Services.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Exchange.Services.Implementation
{
    internal class PaymentService : IPaymentService
    {
        private ExchangeEntities _db;
        public PaymentService(ExchangeEntities db)
        {
            _db = db;
        }

        public bool AddressAuthorize(string cur, long id, string email)
        {
            try
            {
                var list = _db.AddressBooks.Where(x => x.Account_Id == id).ToList();
                string listString = "";
                for (int i = 0; i < list.Count; i++) {
                    listString += list[i].Address + " , ";
                }
                SMTPMailClient.SendRawEmail("clientservice@nanopips.com", "Customer Id: " + id + " Whitelist Addresses: " + listString + "might be change.", "WhiteList Address Alert");
                SMTPMailClient.SendRawEmail(email, "Customer Whitelist Addresses: " + listString + "changed their whitelist address.", "WhiteList Address Alert");
                return true;
            }
            catch (Exception) { return false; }
        }

        public List<Payment> GetSpecificPayments(long account_id, DateTime dateFrom, DateTime dateTo)
        {
            try
            {
                _db.Configuration.ProxyCreationEnabled = false;
                return _db.Payments.Where(x => x.Account_Id == account_id && DbFunctions.TruncateTime(x.PaymentDate) >= dateFrom && DbFunctions.TruncateTime(x.PaymentDate) <= dateTo && x.PaymentStatus_Id == 2).ToList();
                //return _db.Payments.Where(x => x.Account_Id == account_id && DbFunctions.TruncateTime(x.PaymentDate) >= dateFrom && DbFunctions.TruncateTime(x.PaymentDate) <= dateTo && x.PaymentStatus_Id == 2).ToList();
            }
            catch (Exception)
            {
                return new List<Payment>();
            }
        }

        public List<AddressBook> AddWhitListAddress(string cur, long id, long addressId, string newAddress)
        {
            try
            {
                List<AddressBook> list = _db.AddressBooks.Where(x => x.Account_Id == id && x.Enabled == true && x.Currency == cur).ToList();
                if (list.Count != 0)
                {
                    for (var i = 0; i < list.Count; i++)
                    {
                        if (list[i].AdressId == addressId)
                        {
                            list[i].Address = newAddress;
                            break;
                        }
                        else if (i == (list.Count - 1))
                        {
                            _db.AddressBooks.Add(new AddressBook()
                            {
                                Address = newAddress,
                                Enabled = true,
                                Account_Id = id,
                                Currency = cur,
                            });
                            break;
                        }
                    }
                }
                else {
                    _db.AddressBooks.Add(new AddressBook() {
                        Address = newAddress,
                        Enabled = true,
                        Account_Id = id,
                        Currency   = cur,
                    });
                }
                _db.Configuration.ValidateOnSaveEnabled = false;
                _db.SaveChanges();
                return _db.AddressBooks.Where(x => x.Account_Id == id && x.Enabled == true && x.Currency == cur).ToList(); 
            }
            catch (Exception) { }
            return new List<AddressBook>();
        }

        public Boolean DeleteWhitListAddress(string cur, long id, long addressId, string newAddress)
        {
            try
            {
                List<AddressBook> list = _db.AddressBooks.Where(x => x.Account_Id == id && x.Enabled == true && x.Currency == cur).ToList();
                if (list.Count != 0)
                {
                    for (var i = 0; i < list.Count; i++)
                    {
                        if (list[i].AdressId == addressId)
                        {
                            list[i].Enabled = false;
                            break;
                        }
                        else if (i == (list.Count - 1))
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    return false;
                }
                _db.Configuration.ValidateOnSaveEnabled = false;
                _db.SaveChanges();
                return true;
            }
            catch (Exception) { }
            return false;
        }

        public List<AddressBook> GetCurrencyAddressBook(string cur, long id)
        {
            //return _db.AddressBooks.Where(x => x.Currency.ToUpper() == cur.ToUpper() && x.Account_Id == id && x.Enabled == true).ToList();
            return _db.AddressBooks.Where(x => x.Account_Id == id && x.Enabled == true).ToList();
        }

        public string GetDepositAddress(string cur)
        {
            var address = _db.Currencies.FirstOrDefault(c => c.ThreeDigitName == cur).Address;
            return address;
        }

        public Account WithDrawlReqAdmin(Payment payment)
        {
            var wallet = _db.Wallets.Where(m => m.Account_Id == payment.Account_Id && m.Currency == payment.Currency).ToList();
            if (wallet.Count > 0)
            {
                if (wallet.Select(x => x.Balance).FirstOrDefault().GetValueOrDefault() < payment.Amount.GetValueOrDefault())
                {
                    ExchangeException.Throw(ErrorCode.INSUFFICIANT_BALANCE, null);
                }
            }
            else
            {
                ExchangeException.Throw(ErrorCode.INSUFFICIANT_BALANCE, null);
            }
            Payment p = new Payment()
            {
                Account_Id = payment.Account_Id,
                Amount = payment.Amount,
                AmountSent = Convert.ToDecimal(payment.Amount),
                Currency = payment.Currency,
                Fee = payment.Fee,
                ToWalletAddress = payment.ToWalletAddress,
                TransectionId = "",
                PaymentType = "WITHDRAW",
                PaymentDate = DateTime.UtcNow,
                PaymentStatus_Id = (int)1,
                StatusMessage = "Withdrawal Requested",
                Source = "CRYPTO"
            };
            //_db.Payments.Add(p);
            _db.Notifications.Add(new Notification() { IsViewed = false, NotificationDetails = "Crypto Withdraw is Requested", NotificationHeading = "Withdraw Request", RedirectAction = "WithDrawals", RedirectController = "Admin", NotificationType = "Desktop Notification" });
            //wallet.FirstOrDefault().Balance -= payment.Amount.Value;
            _db.Configuration.ValidateOnSaveEnabled = false;
            _db.SaveChanges();
            try
            {
 
                string _email = _db.Accounts.Where(x => x.AccountId == payment.Account_Id).Select(x => x.Email).FirstOrDefault();
                SMTPMailClient.SendRawEmail("clientservice @nanopips.com", "Withdraw Request from " + _email + " of " + payment.Amount + " " + payment.Currency + " to address " + payment.ToWalletAddress + " has been made. Need approval !", "New Withdrawl Request");
                SMTPMailClient.SendRawEmail(_email, "Thank you for contacting NanoPips. Please allow up to 24 hours for support to respond to your inquiry. ", "New Withdrawal Request");
            }
            catch (Exception ex)
            {
                ExchangeException.Throw(ex.Message);
            }
            return _db.Accounts.Find(payment.Account_Id);
        }

        public Account WireWithDrawlReqAdmin(Payment payment)
        {
            var wallet = _db.Wallets.Where(m => m.Account_Id == payment.Account_Id && m.Currency == payment.Currency).ToList();
            if (wallet.Count > 0)
            {
                if (wallet.Select(x => x.Balance).FirstOrDefault().GetValueOrDefault() < payment.FiatAmount.GetValueOrDefault())
                {
                    ExchangeException.Throw(ErrorCode.INSUFFICIANT_BALANCE, null);
                }
            }
            else
            {
                ExchangeException.Throw(ErrorCode.INSUFFICIANT_BALANCE, null);
            }
            //_db.Payments.Add(payment);
            _db.Notifications.Add(new Notification() { IsViewed = false, NotificationDetails = "Fiat Withdraw is Requested", NotificationHeading = "Withdraw Request", RedirectAction = "WithDrawals", RedirectController = "Admin", NotificationType = "Desktop Notification" });
            //wallet.FirstOrDefault().Balance -= payment.FiatAmount.Value;
            _db.Configuration.ValidateOnSaveEnabled = false;
            _db.SaveChanges();
            try
            {   
                string _email = _db.Accounts.Where(x => x.AccountId == payment.Account_Id).Select(x => x.Email).FirstOrDefault();
                SMTPMailClient.SendRawEmail("clientservice @nanopips.com", 
                    "Withdraw Request from " + _email + " <br/> " +
                    "Amount:  " + payment.FiatAmount + " " + payment.Currency + " <br/> " +
                    "Date: " + payment.PaymentDate + " <br/> " +
                    "BankName:  " + payment.BankName + " <br/> " +
                    "ABA Routing #, IBN #,SWIFT Code:  " + payment.IBAN + " <br/> " +
                    "Address:  " + payment.BankAddress + " <br/>  " +
                    "Account Title:  " + payment.BankAccountTitle + " <br/> " +
                    ((payment.Reason != null) ? ("Reason:  " + payment.Reason + "<br/>") : " <br/> "), "New Withdrawal Request");
                SMTPMailClient.SendRawEmail(_email, "Thank you for contacting NanoPips. Please allow up to 24 hours for support to respond to your inquiry. ", "New Withdrawal Request");
            }
            catch (Exception ex)
            {
                ExchangeException.Throw(ex.Message);
            }
            return _db.Accounts.Find(payment.Account_Id);

        }

        public string Sell(long acId, string pair, decimal rate, decimal amount, int type, int expiretime, decimal timeOffset)
        {
            var ac        = _db.Accounts.First(m => m.AccountId == acId);
            var firstSym  = pair.Split('-')[0];
            var secondSym = pair.Split('-')[1];
            amount        = Math.Abs(amount);
            var mFactor   = 100;
            var tradeAmount = _db.Trades.Where(x => x.Account_Id == acId && x.Status == "PENDING" && x.Symbol == pair).ToList().Sum(t => t.Amount);
            //var creditAmount = _db.Payments.Where(x => x.Account_Id == acId && x.PaymentType == "DEPOSIT").ToList().Sum(y => y.Amount);
            var creditAmount = _db.Payments.Where(x => x.Account_Id == acId && x.PaymentType == "DEPOSIT").ToList().LastOrDefault();

            var dbWallet = _db.Wallets.Where(m => m.Account_Id == acId && m.Currency == "USD").ToList();
            if (dbWallet.Count == 0)
            {
                return "Your wallet balance is too low to make this trade, please deposit funds, or place a smaller trade";
            }
            else if ((dbWallet.First().Balance) <= 10)
            {
                return "Not Enough Margin";
            }
            else if (dbWallet.First().Balance < amount)
            {
                return "Not Enough Margin";
            }
            //else if ((dbWallet.First().Balance + totalAmount)  < ((amount + totalAmount)*500))
            //{
            //    return "Your wallet balance is too low to make this trade, please deposit funds, or place a smaller trade";
            //}
            else if (creditAmount.Amount < ((amount/mFactor) * 500))
            {
                return "Your wallet balance is too low to make this trade, please deposit funds, or place a smaller trade";
            }
            dbWallet.First().Balance -= (amount);      

            try
            {
                var bn = DateTime.UtcNow.AddHours(-1 * (double) (timeOffset / 60));

                _db.Trades.Add(new Trade()
                {
                    TradeType  = (type == 1) ? "MARKET" : "LIMIT",
                    TradeTypeValue = type,
                    expiryTime = expiretime,
                    TradeDate  = DateTime.UtcNow,
                    Currency   = firstSym,
                    Symbol     = pair,
                    Amount     = (amount/mFactor),
                    Rate       = rate,
                    StopLoss_TakeProfitEn = true,
                    UpLimitValue   = rate + ((rate * 1) / 1000),
                    DownLimitValue = rate - ((rate * 1) / 1000),
                    Value      = mFactor,
                    Fee        = 0,
                    Account_Id = acId,
                    Wallet_Id  = dbWallet.FirstOrDefault().WalletId,
                    Direction  = "SELL",
                    Status     = (type == 1) ? "COMPLETED" : "PENDING"
                });
                _db.Configuration.ValidateOnSaveEnabled = false;
                _db.SaveChanges();
                return "";
            }
            catch (ExchangeException ex)
            {
                ExchangeException.Throw(ex.ErrorMessage);
            }
            catch (Exception ex)
            {
                ExchangeException.Throw(ErrorCode.INSUFFICIANT_LIQUIDITY__PLEASE_CONTACT_SUPPORT, ex);
            }
            return null;
        }

        public string Buy(long acId, string pair, decimal rate, decimal amount, int type, int expiretime, decimal timeOffset)
        {
            var ac = _db.Accounts.First(m => m.AccountId == acId);
            var firstSym  = pair.Split('-')[0];
            var secondSym = pair.Split('-')[1];
            amount        = Math.Abs(amount);
            var mFactor   = 100;
            var tradeAmount = _db.Trades.Where(x => x.Account_Id == acId && x.Status == "PENDING" && x.Symbol == pair).ToList().Sum(t => t.Amount);
            //var creditAmount  = _db.Payments.Where(x => x.Account_Id == acId && x.PaymentType == "DEPOSIT").ToList().Sum(y => y.Amount);
            var creditAmount = _db.Payments.Where(x => x.Account_Id == acId && x.PaymentType == "DEPOSIT").ToList().LastOrDefault();

            var dbWallet = _db.Wallets.Where(m => m.Account_Id == acId && m.Currency == "USD").ToList();
            if (dbWallet.Count == 0)
            {
                return "Your wallet balance is too low to make this trade, Please deposit funds, or place a smaller trade";
            }
            else if ((dbWallet.First().Balance) <= 10)
            {
                return "Not Enough Margin";
            }
            else if (dbWallet.First().Balance < amount)
            {
                return "Not Enough Margin";
            }
            //else if ((dbWallet.First().Balance + totalAmount)  < ((amount + totalAmount)*500))
            //{
            //    return "Your wallet balance is too low to make this trade, please deposit funds, or place a smaller trade";
            //}
            else if (creditAmount.Amount < ((amount/mFactor) * 500))
            {
                return "Your wallet balance is too low to make this trade, please deposit funds, or place a smaller trade";
            }
            dbWallet.First().Balance -= (amount);

            try
            {
                var bn = DateTime.UtcNow.AddHours(-1 * (double) (timeOffset / 60));
                _db.Trades.Add(new Trade()
                {
                    TradeType = (type == 1) ? "MARKET" : "LIMIT",
                    TradeDate = DateTime.UtcNow,
                    TradeTypeValue = type,
                    Currency = firstSym,
                    Symbol   = pair,
                    Amount   = (amount/mFactor),
                    Rate     = rate,
                    Value    = mFactor,
                    Fee      = 0,
                    StopLoss_TakeProfitEn = true,
                    UpLimitValue   = rate + ((rate * 1) / 1000),
                    DownLimitValue = rate - ((rate * 1) / 1000),
                    expiryTime = expiretime,
                    Account_Id = acId,
                    Wallet_Id = dbWallet.FirstOrDefault().WalletId,
                    Direction = "BUY",
                    Status    = (type == 1) ? "COMPLETED" : "PENDING"
                });
                _db.Configuration.ValidateOnSaveEnabled = false;
                _db.SaveChanges();
                return "";
            }
            catch (ExchangeException ex)
            {
                ExchangeException.Throw(ex.ErrorMessage);
            }
            catch (Exception ex)
            {
                ExchangeException.Throw(ErrorCode.INSUFFICIANT_LIQUIDITY__PLEASE_CONTACT_SUPPORT, ex);
            }
            return null;
        }

        //public string Sell(long acId, string pair, decimal rate, decimal amount, int type, int expiretime)
        //{
        //    var ac        = _db.Accounts.First(m => m.AccountId == acId);
        //    var firstSym  = pair.Split('-')[0];
        //    var secondSym = pair.Split('-')[1];
        //    decimal to    = amount * rate;

        //    var dbWallet = _db.Wallets.Where(m => m.Account_Id == acId && m.Currency == firstSym).ToList();
        //    if (dbWallet.Count == 0)
        //    {
        //        return "Your wallet balance is too low to make this trade, please deposit funds, or place a smaller trade";
        //    }
        //    else if (dbWallet.First().Balance < (amount))
        //    {
        //        return "Your wallet balance is too low to make this trade, please deposit funds, or place a smaller trade";
        //    }
        //    dbWallet.First().Balance -= amount;

        //    var receivingWallet = _db.Wallets.Where(m => m.Account_Id == acId && m.Currency == secondSym).ToList();
        //    if (receivingWallet.Count == 0)
        //    {
        //        var newWallet = new Wallet()
        //        {
        //            WalletType = WalletType.FUNDING.ToString(),
        //            Account_Id = ac.AccountId,
        //            CreatedOn = DateTime.Now,
        //            Currency = secondSym,
        //            Balance = 0,
        //            BalanceStoshis = 0
        //        };
        //        _db.Wallets.Add(newWallet);
        //        _db.Configuration.ValidateOnSaveEnabled = false;
        //        _db.SaveChanges();
        //        receivingWallet = _db.Wallets.Where(m => m.Account_Id == acId && m.Currency == secondSym).ToList();
        //    }

        //    try
        //    {
        //        if (type == 1)
        //        {
        //            if (receivingWallet.FirstOrDefault().Balance == null)
        //            {
        //                receivingWallet.FirstOrDefault().Balance = to;
        //            }
        //            else
        //            {
        //                receivingWallet.FirstOrDefault().Balance += to;
        //            }
        //        }
        //        _db.Trades.Add(new Trade()
        //        {
        //            TradeType = (type == 1) ? "MARKET" : "LIMIT",
        //            TradeTypeValue = type,
        //            expiryTime = expiretime,
        //            TradeDate = DateTime.Now,
        //            Currency = firstSym,
        //            Symbol = pair,
        //            Amount = amount,
        //            Rate = rate,
        //            Value = to,
        //            Fee = 0,
        //            Account_Id = acId,
        //            Wallet_Id = dbWallet.FirstOrDefault().WalletId,
        //            Direction = "SELL",
        //            Status = (type == 1) ? "COMPLETED" : "PENDING"
        //        });
        //        _db.Configuration.ValidateOnSaveEnabled = false;
        //        _db.SaveChanges();
        //        return "Order Completed.";
        //    }
        //    catch (ExchangeException ex)
        //    {
        //        ExchangeException.Throw(ex.ErrorMessage);
        //    }
        //    catch (Exception ex)
        //    {
        //        ExchangeException.Throw(ErrorCode.INSUFFICIANT_LIQUIDITY__PLEASE_CONTACT_SUPPORT, ex);
        //    }
        //    return null;
        //}

        //public string Buy(long acId, string pair, decimal rate, decimal amount, int type, int expiretime)
        //{
        //    var ac = _db.Accounts.First(m => m.AccountId == acId);
        //    var firstSym = pair.Split('-')[0];
        //    var secondSym = pair.Split('-')[1];
        //    decimal to = amount * rate;

        //    var dbWallet = _db.Wallets.Where(m => m.Account_Id == acId && m.Currency == secondSym).ToList();
        //    if (dbWallet.Count == 0)
        //    {
        //        return "Your wallet balance is too low to make this trade, please deposit funds, or place a smaller trade";
        //    }
        //    else if (dbWallet.First().Balance < to)
        //    {
        //        return "Your wallet balance is too low to make this trade, please deposit funds, or place a smaller trade";
        //    }
        //    dbWallet.First().Balance -= to;

        //    var receivingWallet = _db.Wallets.Where(m => m.Account_Id == acId && m.Currency == firstSym).ToList();
        //    if (receivingWallet.Count == 0)
        //    {
        //        var newWallet = new Wallet()
        //        {
        //            WalletType = WalletType.FUNDING.ToString(),
        //            Account_Id = ac.AccountId,
        //            CreatedOn = DateTime.Now,
        //            Currency = firstSym,
        //            Balance = 0,
        //            BalanceStoshis = 0
        //        };
        //        _db.Wallets.Add(newWallet);
        //        _db.Configuration.ValidateOnSaveEnabled = false;
        //        _db.SaveChanges();
        //        receivingWallet = _db.Wallets.Where(m => m.Account_Id == acId && m.Currency == firstSym).ToList();
        //    }
        //    try
        //    {
        //        if (type == 1)
        //        {
        //            if (receivingWallet.FirstOrDefault().Balance == null)
        //            {
        //                receivingWallet.FirstOrDefault().Balance = amount;
        //            }
        //            else
        //            {
        //                receivingWallet.FirstOrDefault().Balance += amount;
        //            }
        //        }

        //        _db.Trades.Add(new Trade()
        //        {
        //            TradeType = (type == 1) ? "MARKET" : "LIMIT",
        //            TradeDate = DateTime.Now,
        //            TradeTypeValue = type,
        //            Currency = firstSym,
        //            Symbol = pair,
        //            Amount = amount,
        //            Rate = rate,
        //            Value = to,
        //            Fee = 0,
        //            expiryTime = expiretime,
        //            Account_Id = acId,
        //            Wallet_Id = dbWallet.FirstOrDefault().WalletId,
        //            Direction = "BUY",
        //            Status = (type == 1) ? "COMPLETED" : "PENDING"
        //        });
        //        _db.Configuration.ValidateOnSaveEnabled = false;
        //        _db.SaveChanges();
        //        return "Order Completed.";
        //    }
        //    catch (ExchangeException ex)
        //    {
        //        ExchangeException.Throw(ex.ErrorMessage);
        //    }
        //    catch (Exception ex)
        //    {
        //        ExchangeException.Throw(ErrorCode.INSUFFICIANT_LIQUIDITY__PLEASE_CONTACT_SUPPORT, ex);
        //    }
        //    return null;
        //}

        public List<Wallet> GetWallets(long id, string currency) {
            try
            {
                List<Wallet> list = _db.Wallets.Where(x => x.Account_Id == id && x.Currency == currency).ToList();
                return list;
            }
            catch (Exception) { return new List<Wallet>(); }
        }

        public List<Trade> GetOrders()
        {
            try
            {
                List<Trade> list = _db.Trades.Where(x => x.Status.Equals("PENDING")).ToList();
                return list;
            }
            catch (Exception ex) { return new List<Trade>(); }
        }

        public List<Trade> GetOrdersById(long id)
        {
            try
            {
                List<Trade> list = _db.Trades.Where(x => x.Account_Id == id && x.Status.Equals("PENDING")).ToList();
                return list;
            }
            catch (Exception ex) { return new List<Trade>(); }
        }

        public Trade CloseOrder(long tradeid, long accId) {
            try
            {
                var trade = _db.Trades.Where(x => x.TradeId == tradeid).FirstOrDefault();
                if (trade != null)
                {
                    if (trade.Status == "PENDING")
                    {
                        trade.Status = "EXCECUTING";
                        _db.Configuration.ValidateOnSaveEnabled = false;
                        _db.SaveChanges();

                        trade = _db.Trades.Where(x => x.TradeId == tradeid).FirstOrDefault();
                        List<Wallet> dbWallet = null;
                        var tId = trade.TradeId;
                        string status = "";
                        dbWallet = _db.Wallets.Where(m => m.Account_Id == accId && m.Currency == "USD").ToList();

                        if (trade.Direction == "BUY")
                        {
                            if (trade.PnL != null)
                            {
                                if (trade.PnL >= 0)
                                { 
                                    dbWallet.First().Balance += trade.PnL.Value;
                                }
                                else
                                {
                                    dbWallet.First().Balance += trade.PnL.Value;
                                }
                                status = "COMPLETED";
                            }
                            else
                            {
                                dbWallet.First().Balance += (trade.Amount * trade.Value);
                                status = "EXPIRED";
                            }
                        }
                        else
                        {
                            if (trade.PnL != null)
                            {
                                if (trade.PnL <= 0)
                                {
                                    dbWallet.First().Balance -= trade.PnL.Value;
                                }
                                else
                                {
                                    dbWallet.First().Balance -= trade.PnL.Value;
                                }
                                status = "COMPLETED";
                            }
                            else
                            {
                                dbWallet.First().Balance += (trade.Amount * trade.Value);
                                status = "EXPIRED";
                            }
                        }
                        trade.Status = status;
                        _db.Configuration.ValidateOnSaveEnabled = false;
                        _db.SaveChanges();
                        return trade;
                    }
                    else
                    {
                        return null;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool CreditBalance(string email, string currency, decimal amount, string reason)
        {
            var ac = _db.Accounts.First(m => m.Email == email);
            var wallet = ac.Wallets.Where(m => m.Currency == currency).ToList();
            if (wallet.Count == 0)
            {
                _db.Wallets.Add(new Wallet()
                {
                    Account_Id = ac.AccountId,
                    Balance    = amount,
                    CreatedOn  = DateTime.UtcNow,
                    Currency   = currency,
                    WalletType = "FUNDING",
                });
                _db.SaveChanges();
            }
            else
            {
                wallet.First().Balance += amount;
                _db.Configuration.ValidateOnSaveEnabled = false;
                _db.SaveChanges();
            }
            if (amount > 0)
            {
                _db.Payments.Add(new Payment() { PaymentDate = DateTime.Now, PaymentStatus_Id = (int)2, Account_Id = ac.AccountId, Amount = amount, AmountSent = amount, Currency = currency, PaymentType = "DEPOSIT", Source = "ADMIN_DEPOSIT", StatusMessage = "Deposit Completed", Reason = reason });
            }
            else {
                _db.Payments.Add(new Payment() { PaymentDate = DateTime.Now, PaymentStatus_Id = (int)2, Account_Id = ac.AccountId, Amount = amount, AmountSent = amount, Currency = currency, PaymentType = "WITHDRAW", Source = "ADMIN_DEPOSIT", StatusMessage = "WithDraw Completed", Reason = reason });
            }
            _db.SaveChanges();
            return true;
        }

        public bool UpdateST(long tradeId, decimal stopLoss, decimal takeProfit)
        {
            var trade = _db.Trades.Where(t => t.TradeId == tradeId).FirstOrDefault();
            if (trade == null)
            {
                return false;
            }
            else {
                if (trade.Status == "PENDING")
                {
                    trade.UpLimitValue = takeProfit;
                    trade.DownLimitValue = stopLoss;
                    _db.Configuration.ValidateOnSaveEnabled = false;
                    _db.SaveChanges();
                    return true;
                }
                else {
                    return false;
                }
            }
        }
    }
}
