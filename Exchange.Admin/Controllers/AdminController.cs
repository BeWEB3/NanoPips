using Exchange.Admin.Models;
using CoinEXR.Admin.App_Start;
using Exchange.Common;
using Exchange.DTO;
using Exchange.UI.Models;
using Exchange.UOW;
using MVCGrid.Models;
using MVCGrid.Web;
using Ninject;
using System;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.Mvc;
using CoinEXR.Admin;
using CoinEXR.Admin.Models;
using System.Collections.Generic;

namespace Exchange.UI.Controllers
{

    [AdminAuthorize]
    public class AdminController : Controller
    {
        private IUnitOfWork _uow;
        public AdminController(IUnitOfWork _uow)
        {
            this._uow = _uow;
        }
        // GET: Admin
        public ActionResult Index()
        {
            var ac = SessionItems.Get(SessionKey.ACCOUNT) as Account;
            //Session["pin"] = _uow.Accounts.GetPinCode(ac.AccountId);
            DashboardModel dashboard = new DashboardModel();
            dashboard.TotalAccounts = _uow.Accounts.Get().Where(m => m.AccountType_Id != 5).Count();
            dashboard.TotalSystemAccounts = _uow.Accounts.Get().Where(m => m.AccountType_Id == 5).Count();
            dashboard.TotalCurrencies = _uow.MarketRates.GetAllCurrencies().Count();
            // dashboard.TotalProfit = _uow.Payment.GetAll().Where(m => m.PaymentType == PaymentType.BUY.ToString() || m.PaymentType == PaymentType.SELL.ToString()).Sum(m=>m.Revenue).Value.ToString("0.0000");

            var adminAccounts = _uow.Accounts.Get().Where(m => m.AccountType_Id == 5).Take(5);

            AdminAccounts admin = new AdminAccounts();
            foreach (var item in adminAccounts)
            {
                admin = new AdminAccounts();
                admin.firstName = item.FirstName;
                admin.lastName  = item.LastName;
                admin.email     = item.Email;
                admin.accountType = item.UserRoles.FirstOrDefault().UserRoleType.RoleType;
                dashboard.adminAccountList.Add(admin);
            }
            return View(dashboard);
        }

        public ActionResult notification(string notid)
        {
            var not = _uow.Accounts.UpdateNotification(Convert.ToInt64(notid));
            // return redirecttoaction(not.redirectaction, not.redirectcontroller);
            return Redirect("/" + not.RedirectController + "/" + not.RedirectAction);
        }

        public ActionResult CreditBalance(string cur, string bal, string acId)
        {
            long id = Convert.ToInt64(acId);
            var aco = SessionItems.Get(SessionKey.ACCOUNT) as Account;
            ViewBag.Id = id;
            ViewBag.currency = cur;
            ViewBag.walletBalance = bal;
            var ac = _uow.Accounts.Get().Where(m => m.AccountId == id).First();
            ViewBag.account = ac.Email;
            return View();
        }

        public ActionResult _AddRefferal(string email, long accounId)
        {
            var ac = SessionItems.Get(SessionKey.ACCOUNT) as Account;
            if (ac == null)
            {
                return RedirectToAction("Login", "Account");
            }
            try
            {
                var msg = _uow.Payment.AddRefferal(email, accounId);
                return Json(new { msg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex) {
                var msg = "An Error Occured";
                return Json(new { msg }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult _refferalSubtract(decimal amount, long accounId, string reason)
        {
            var ac = SessionItems.Get(SessionKey.ACCOUNT) as Account;
            if (ac == null)
            {
                return RedirectToAction("Login", "Account");
            }
            try
            {
                var msg = _uow.Payment.refferalSubtract(amount, accounId, reason);
                return Json(new { msg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex) {
                var msg = "An Error Occured";
                return Json(new { msg }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult CreditBalance(string acId, string email, string currency, double amountusd = 0, double amountbtc = 0, string reason = "")
        {
            double amount = 0;
            if (amountbtc != 0) {
                amount = amountbtc;
            }
            else if (amountusd != 0) {
                amount = amountusd;
            }
            try
            {
                _uow.Payment.CreditBalance(email, "USD", (decimal)amount, reason);
                TempData["msg"] = "Account " + email + " has been successfully toped up with " + amount + " " + currency + " by admin";
            }
            catch (Exception ex)
            {
                TempData["msg"] = "An error has occured, please try again later";
            }
            return RedirectToAction("AccountDetail", "Admin", new { acId = acId });
        }

        public ActionResult _GetTradeHistory(long accId)
        {
            DateTime from, to;
            from = DateTime.UtcNow.AddYears(-20);
            to = DateTime.UtcNow;

            var ac = SessionItems.Get(SessionKey.ACCOUNT) as Account;
            var paymentsList = _uow.Payment.GetSpecificPayments(accId, from, to);
            var res = _uow.Accounts.GetTradeHistory(accId, from, to);
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

        public ActionResult _GetBelow10Losses()
        {
            var ac = SessionItems.Get(SessionKey.ACCOUNT) as Account;
            decimal? totalLoses = null;
            totalLoses = _uow.Accounts.GetTotalUsersLoses();
            return Json(new { totalLoses }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult _chartData()
        {
            int year = DateTime.Now.Year;
            var data = _uow.Accounts.Get().Where(m => m.CreationDate.Value.Year == year).GroupBy(m => new { month = m.CreationDate.Value.Month }).ToList().Select(m => new { Total = m.Count(), Date = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(m.Key.month) }).ToList();
            return Json(new { Total = data.Select(m => m.Total).ToArray(), Date = data.Select(m => m.Date).ToArray() }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AccountDetail(string acId)
        {
            long id = Convert.ToInt64(acId);
            ViewBag.acDetId = id;
            ViewBag.currencies = _uow.MarketRates.GetAllCurrencies().Where(x => x.Status == true && x.ThreeDigitName == "USD").ToList();
            return View(_uow.Accounts.Get().Where(m => m.AccountId == id).First());

        }

        [HttpPost]
        public string _ReadNotification()
        {
            _uow.Accounts.ReadAllNotification();
            return "ok";
        }

        public ActionResult ResetTwofa(string acId)
        {
            try
            {
                _uow.Accounts.ResetTwoFA(Convert.ToInt64(acId));
                TempData["msg"] = "TwoFA has been disabled successfully.";
            }
            catch
            {
                TempData["msg"] = "An error has occured, please try again later";

            }
            return RedirectToAction("AccountDetail", "Admin", new { acId });
        }

        public ActionResult _notification()
        {
            return View(_uow.Accounts.GetNotifications());
        }

        public ActionResult Accounts()
        {
            return View();
        }

        public ActionResult AccountsProfit()
        {
            return View();
        }

        public ActionResult AccountsLoss()
        {
            return View();
        }

        //    public ActionResult AdminRejectedWithdraw(long pId, string currency, string address, double amount)
        //    {
        //        try
        //        {
        //            var response = _uow.Payment.AdminRejectedWithdrawal(pId, currency, address, amount);
        //            TempData["msg"] = "Withdrawal has been rejected successfully";
        //            return RedirectToAction("Index", "Admin");
        //        }
        //        catch (Exception ex)
        //        {
        //            TempData["msg"] = ex.Message;
        //            return RedirectToAction("WithDrawls", "Admin");
        //        }
        //    }
        //    public ActionResult Settings()
        //    {
        //        var set = _uow.Payment.GetAdminSettings().ToList();
        //        SettingModel model = new SettingModel()
        //        {
        //            BuyPrice = Convert.ToDecimal(set.Where(m => m.SettingKey == AdminSettingKey.BUY_PRICE.ToString()).First().SettingValue),
        //            SellPrice = Convert.ToDecimal(set.Where(m => m.SettingKey == AdminSettingKey.SELL_PRICE.ToString()).First().SettingValue),
        //            Network = set.Where(m => m.SettingKey == AdminSettingKey.NETWORK.ToString()).First().SettingValue,
        //            WithdrawLimit = Convert.ToDecimal(set.Where(m => m.SettingKey == AdminSettingKey.WITHDRAW_LIMIT.ToString()).First().SettingValue),
        //            MinimumLimit = Convert.ToDecimal(set.Where(m => m.SettingKey == AdminSettingKey.MINIMUM_LIMIT.ToString()).First().SettingValue),
        //            ExchangeStatus = set.Where(m => m.SettingKey == AdminSettingKey.EXCHANGE_STATUS.ToString()).First().SettingValue,
        //            CardFee = set.Where(m => m.SettingKey == AdminSettingKey.CARD_FEE.ToString()).First().SettingValue,
        //            TradeFee = Convert.ToDecimal(set.Where(m => m.SettingKey == AdminSettingKey.COMMISSION.ToString()).First().SettingValue),
        //        };
        //        return View(model);
        //    }
        //    [HttpPost]
        //    public ActionResult Settings(SettingModel model)
        //    {
        //        _uow.Payment.SetAdminSettings(model.BuyPrice, model.SellPrice, model.Network, model.WithdrawLimit, model.MinimumLimit, model.ExchangeStatus,model.CardFee,model.TradeFee);
        //        TempData["msg"] = "Setting has been updated successfully";
        //        return RedirectToAction("Index", "Admin");
        //    }

        public ActionResult SuspendAccount(string acId)
        {
            _uow.Accounts.Suspend(Convert.ToInt64(acId));
            TempData["msg"] = "Account has been suspended successfully";
            return RedirectToAction("Accounts", "Admin");

        }
        public ActionResult ActivateAccount(string acId)
        {
            _uow.Accounts.Activate(Convert.ToInt64(acId));
            TempData["msg"] = "Account has been activated successfully";
            return RedirectToAction("Accounts", "Admin");
        }
        public ActionResult ApproveAccount(string acId)
        {
            _uow.Accounts.Approve(Convert.ToInt64(acId));
            TempData["msg"] = "Account has been approved successfully";
            return RedirectToAction("AccountDetail", "Admin", new { acId = acId });
        }
        public ActionResult RejectAccount(string acId, string reason)
        {
            _uow.Accounts.Reject(Convert.ToInt64(acId), reason);
            TempData["msg"] = "Documents have been rejected successfully";
            return RedirectToAction("AccountDetail", "Admin", new { acId = acId });
        }
        //    public ActionResult Deposits()
        //    {
        //        return View();
        //    }
        //    public ActionResult WithDrawls()
        //    {
        //        ViewBag.requestedWithdrawals = _uow.Accounts.GetRequestedWithdrawals();
        //        return View();
        //    }
        //    public ActionResult BuySells()
        //    {
        //        return View();
        //    }
        //    public ActionResult Profit()
        //    {
        //        return View();
        //    }
        //    public async Task<ActionResult> Wallet()
        //    {
        //        var ac = SessionItems.Get(SessionKey.ACCOUNT) as Account;
        //        ViewBag.masterBalances = await _uow.MarketRates.GetMasterWalletsBalances();
        //        return View(_uow.MarketRates.GetCurrencies().ToList());
        //    }
        //    public bool VerifyTwoFA(string twofaCode)
        //    {
        //        if (SessionItems.Get(SessionKey.ACCOUNT) == null)
        //        {
        //            return false;
        //        }
        //        try
        //        {
        //            _uow.Accounts.Authenticate2FACode((SessionItems.Get(SessionKey.ACCOUNT) as Account).AccountId, twofaCode);
        //            SessionItems.Add(SessionKey.TWOFA, true);
        //            return true;
        //        }
        //        catch
        //        {
        //            return false;
        //        }

        //    }

        //    public ActionResult WalletBalances()
        //    {
        //        ViewBag.currencies = _uow.MarketRates.GetCurrencies().ToList();
        //        return View(_uow.Accounts.GetWallets());
        //    }
        //    public string _profitTotal(string paymentdate = "", string paymenttype = "")
        //    {
        //        var query = _uow.Payment.GetAll().Where(m => m.PaymentType == PaymentType.BUY.ToString() && m.PaymentType == PaymentType.SELL.ToString());
        //        string date = paymentdate;
        //        if (!string.IsNullOrEmpty(date) && date != "null")
        //        {
        //            DateTime fromDate = new DateTime(Convert.ToInt32(date.Split('-')[0].Trim().Split('/')[2]), Convert.ToInt32(date.Split('-')[0].Trim().Split('/')[0]), Convert.ToInt32(date.Split('-')[0].Trim().Split('/')[1]));
        //            DateTime toDate = new DateTime(Convert.ToInt32(date.Split('-')[1].Trim().Split('/')[2]), Convert.ToInt32(date.Split('-')[1].Trim().Split('/')[0]), Convert.ToInt32(date.Split('-')[1].Trim().Split('/')[1]));
        //            query = query.Where(m => DbFunctions.TruncateTime(m.PaymentDate) >= fromDate && DbFunctions.TruncateTime(m.PaymentDate) <= toDate);

        //        }
        //        string type = paymenttype;
        //        if (!string.IsNullOrEmpty(type) && type != "null")
        //        {
        //            query = query.Where(m => m.PaymentType == type);
        //        }
        //        var sum = query.Sum(m => m.Revenue);
        //        return (sum == null) ? "0.0000" : sum.Value.ToString("0.0000");
        //    }
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePassword model)
        {
            try
            {
                var ac = SessionItems.Get(SessionKey.ACCOUNT) as Account;
                _uow.Accounts.ChangePassword(ac.AccountId, model.OldPassword, model.NewPassword);
                TempData["msg"] = "Password has been updated successfully";
                return RedirectToAction("Index", "Admin");
            }
            catch (ExchangeException ex)
            {
                ViewBag.msg = ex.ErrorMessage;
                return View();
            }
        }

        //    public ActionResult Currency()
        //    {
        //        return View();
        //    }

        public ActionResult ActivateCurrency(string curId)
        {
            _uow.MarketRates.ActivateCurrency(Convert.ToInt64(curId));
            TempData["msg"] = "Currency has been activated successfully";
            return RedirectToAction("Currency", "Admin");
        }

        public ActionResult DeactivateCurrency(string curId)
        {
            _uow.MarketRates.DeactivateCurrency(Convert.ToInt64(curId));
            TempData["msg"] = "Currency has been deactivated successfully";
            return RedirectToAction("Currency", "Admin");
        }

        public ActionResult AdminAccounts()
        {
            return View();
        }

        public ActionResult CreateAdminAccount()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateAdminAccount(AdminAccountModel model)
        {
            _uow.Accounts.CreateAdminAccount(model.AccountType, model.FirstName, model.LastName, model.Email, model.Password);
            TempData["msg"] = "Admin account has been created successfully";
            return RedirectToAction("AdminAccounts", "Admin");
        }

        //public ActionResult CompletePayment(string pId, string Email)
        //{
        //    string[] str = new string[1];
        //    str[0] = Email;
        //    _uow.Payment.CompleteManualPayment(Convert.ToInt64(pId));
        //    TempData["msg"] = "Payment status has been updated to completed";
        //    SMTPMailClient.SendEmail("Payment status has been completed.", "Payment Status", str);
        //    return RedirectToAction("WithDrawls", "Admin");
        //}
        //    public ActionResult RejectPayment(string pId, string Email)
        //    {
        //        string[] str = new string[1];
        //        str[0] = Email;
        //        _uow.Payment.RejectPayment(Convert.ToInt64(pId));
        //        SMTPMailClient.SendEmail("Payment status has been updated to rejected and amount revert back to user", "Payment Status", str);
        //        TempData["msg"] = "Payment status has been rejected and amount is reverted back.";
        //        return RedirectToAction("WithDrawls", "Admin");
        //    }

        //    public ActionResult TransactionHistoryForAdmin()
        //    {
        //        return View(_uow.Payment.GetTransactiontHistory());
        //    }
        //    public ActionResult EditAdminAccount(string acId)
        //    {
        //        long id = Convert.ToInt64(acId);
        //        var ac = _uow.Accounts.Get().Where(m => m.AccountId == id).First();

        //        AdminAccountModel model = new AdminAccountModel()
        //        {
        //            AccountId = (int)id,
        //            FirstName = ac.FirstName,
        //            LastName = ac.LastName,
        //            Email = ac.Email,
        //            Password = ac.UserRoles.First().Password,
        //            AccountType = Convert.ToString(ac.UserRoles.First().UserRoleTypeId)
        //        };


        //        return View(model);
        //    }
        //    public ActionResult TwoFactorAuthentication()
        //    {
        //        var _ac = SessionItems.Get(SessionKey.ACCOUNT) as Account;
        //        var ac = _uow.Accounts.GetAccount(_ac.AccountId);

        //        ViewBag.barCodeUrl = _uow.Accounts.Get2FABarCodeURL(ac.Email);
        //        return View(ac);
        //    }
        //    [HttpPost]
        //    public ActionResult Verify2FACode(string code)
        //    {
        //        var _ac = SessionItems.Get(SessionKey.ACCOUNT) as Account;
        //        var ac = _uow.Accounts.GetAccount(_ac.AccountId);
        //        try
        //        {
        //            var account = _uow.Accounts.Authenticate2FACode(ac.AccountId, code);
        //            SessionItems.Add(SessionKey.ACCOUNT, account);
        //            TempData["msg"] = "Two factor authentication has been enabled successfully.";
        //            return RedirectToAction("Index", "Admin");
        //        }
        //        catch (ExchangeException ex)
        //        {

        //            TempData["msg"] = ex.ErrorMessage;
        //            return RedirectToAction("TwoFactorAuthentication", "Admin");
        //        }
        //    }

        [HttpPost]
        public ActionResult EditAdminAccount(AdminAccountModel model)
        {
            _uow.Accounts.EditAdminAccount(model.AccountId, model.AccountType, model.FirstName, model.LastName);
            TempData["msg"] = "Admin account has been updated successfully";
            return RedirectToAction("AdminAccounts", "Admin");
        }

        public ActionResult EditUserAccount(string acId)
        {
            long id = Convert.ToInt64(acId);
            var ac = _uow.Accounts.Get().Where(m => m.AccountId == id).First();

            UserAccountModel model = new UserAccountModel()
            {
                AccountId = (int)id,
                FirstName = ac.FirstName,
                LastName = ac.LastName,
                Email = ac.Email,
                Address = ac.Address1 + ac.Address2 + ac.Country,
                City = ac.City,
                Country = ac.Country,
                PlaceOfBirth = ac.PlaceOfBirth
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult EditUserAccount(UserAccountModel model)
        {
            _uow.Accounts.EditUserAccount(model.AccountId, model.FirstName, model.LastName, model.Address, model.City, model.Country, model.PlaceOfBirth);
            TempData["msg"] = "User account has been updated successfully";
            return RedirectToAction("AccountDetail", "Admin", new { acId = model.AccountId });
        }

        public ActionResult SuspendAdminAccount(string acId)
        {
            _uow.Accounts.Suspend(Convert.ToInt64(acId));
            TempData["msg"] = "Account has been suspended successfully";
            return RedirectToAction("AdminAccounts", "Admin");

        }
        public ActionResult ActivateAdminAccount(string acId)
        {
            _uow.Accounts.Activate(Convert.ToInt64(acId));
            TempData["msg"] = "Account has been activated successfully";
            return RedirectToAction("AdminAccounts", "Admin");
        }

        public FileResult DownloadFile(string url)
        {

            string filepath = Server.MapPath(url);
            byte[] filedata = System.IO.File.ReadAllBytes(filepath);
            string contentType = MimeMapping.GetMimeMapping(filepath);
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = url.Split('/')[url.Split('/').Length - 1],
                Inline = true,
            };
            Response.AppendHeader("Content-Disposition", cd.ToString());
            return File(filedata, contentType);
        }
    }
        public class AdminGrids : GridRegistration
        {
            public override void RegisterGrids()
            {
                #region Accounts
                MVCGridDefinitionTable.Add("AccountsGrid", new MVCGridBuilder<Account>(MVCGridConfig.GridDefaults, MVCGridConfig.ColumnDefaults)
                 .AddColumns(c =>
                  {
                      //c.Add("CreationDate").WithValueExpression(m=>m.CreationDate.Value.ToString("dd MMMM, yyyy")).WithHeaderText("CreationDate").WithAllowChangeVisibility(true).WithFiltering(true);
                      c.Add("RefferenceNumber").WithValueExpression(m =>
                      {
                          string msg = "";
                          msg += "<span class='btn'><a style='color:#000' href='/Admin/AccountDetail?acId=" + m.AccountId + "'>" + m.RefferenceNumber + "</a></span>";
                          return msg;
                      }).WithHeaderText("RefNumber").WithHtmlEncoding(false).WithAllowChangeVisibility(true).WithFiltering(true).WithSorting(false);
                      c.Add("FirstName").WithValueExpression(m =>
                      {
                          string msg = "";
                          msg += "<span class='btn'><a style='color:#000' href='/Admin/AccountDetail?acId=" + m.AccountId + "'>" + m.FirstName + "</a></span>";
                          return msg;
                      }).WithHeaderText("FirstName").WithHtmlEncoding(false).WithAllowChangeVisibility(true).WithFiltering(true).WithSorting(true);
                      c.Add("LastName").WithValueExpression(m =>
                      {
                          string msg = "";
                          msg += "<span class='btn'><a style='color:#000' href='/Admin/AccountDetail?acId=" + m.AccountId + "'>" + m.LastName + "</a></span>";
                          return msg;
                      }).WithHeaderText("LastName").WithHtmlEncoding(false).WithAllowChangeVisibility(true).WithFiltering(true).WithSorting(true);
                      c.Add("Email").WithValueExpression(m =>
                      {
                          string msg = "";
                          msg += "<span class='btn'><a style='color:#000' href='/Admin/AccountDetail?acId=" + m.AccountId + "'>" + m.Email + "</a></span>";
                          return msg;
                      }).WithHeaderText("Email").WithHtmlEncoding(false).WithAllowChangeVisibility(true).WithFiltering(true).WithSorting(true);
                      c.Add("Password").WithValueExpression(m => m.UserRoles.First().Password).WithHeaderText("Password").WithAllowChangeVisibility(false).WithFiltering(false);
                      //c.Add("DOBDay").WithValueExpression(m => (m.DOBDay == null) ? "" : m.DOBYear + "-" + m.DOBMonth + "-" + m.DOBDay).WithHeaderText("Date of Birth").WithAllowChangeVisibility(true).WithFiltering(true).WithSorting(false);
                      c.Add("Status").WithValueExpression(m =>
                      {
                          string msg = "";
                          if (m.AccountType_Id == (int)AccountTypes.NEW)
                          {
                              msg += "<span class='btn-danger btn' style='width:126px'><a style='color:#fff' href='/Admin/AccountDetail?acId=" + m.AccountId + "'> NOT VERIFIED </a></span>";
                          }
                          else if (m.AccountType_Id == (int)AccountTypes.BASIC)
                          {
                              msg += "<span class='btn-warning btn' style='width:145px'><a style='color:#fff' href='/Admin/AccountDetail?acId=" + m.AccountId + "'> DOCS UPLOADED </a> </span>";
                          }
                          else if (m.AccountType_Id == (int)AccountTypes.REJECTED)
                          {
                              msg += "<span class='btn btn-danger' style='width:126px'><a style='color:#fff' href='/Admin/AccountDetail?acId=" + m.AccountId + "'> REJECTED </a> </span>";

                          }
                          else if (m.AccountType_Id == (int)AccountTypes.VERIFIED)
                          {
                              msg += "<span class='btn-success btn' style='width:126px'><a style='color:#fff' href='/Admin/AccountDetail?acId=" + m.AccountId + "'> VERIFIED </a></span>";

                          }
                          return msg;
                      }).WithHeaderText("Status").WithHtmlEncoding(false).WithAllowChangeVisibility(true).WithFiltering(true).WithSorting(false);

                      c.Add("Options").WithValueExpression(m =>
                      {

                          string html = @"<span>";
                          if (m.AccountType_Id == (int)AccountTypes.BASIC)
                          {
                              html += @"<a href=""/Admin/AccountDetail?acId=" + m.AccountId + @""" title=""Documents uploaded. View details"" class=""btn btn-sm btn-success""><i class=""fa fa-user""></i></a>";
                          }
                          else if (m.AccountType_Id == (int)AccountTypes.REJECTED)
                          {
                              html += @"<a href=""/Admin/AccountDetail?acId=" + m.AccountId + @""" title=""Documents rejected. View details"" class=""btn btn-sm btn-danger""><i class=""fa fa-user""></i></a>";
                          }
                          else
                          {
                              html += @"<a href=""/Admin/AccountDetail?acId=" + m.AccountId + @""" title=""View details"" class=""btn btn-sm btn-default""><i class=""fa fa-user""></i></a>";

                          }
                          if (m.AccountEnabled == true)
                          {
                              html += @" | <a href=""/Admin/SuspendAccount?acId=" + m.AccountId + @""" title=""suspend"" class=""btn btn-sm btn-default suspendAccount""><i class=""fa fa-lock""></i></a>";
                          }
                          else
                          {
                              html += @" | <a href=""/Admin/ActivateAccount?acId=" + m.AccountId + @""" title=""activate"" class=""btn btn-sm btn-default activateAccount""><i class=""fa fa-unlock""></i></a>";

                          }

                          html += "</span>";
                          return html;

                      }).WithFiltering(false).WithSorting(false).WithHtmlEncoding(false);

                  }).WithDefaultSortColumn("AccountId").WithDefaultSortDirection(SortDirection.Dsc).WithAdditionalQueryOptionName("search")
                 .WithRetrieveDataMethod((context) =>
                               {

                                   var result = new QueryResult<Account>();
                                   var options = context.QueryOptions;
                                   IUnitOfWork uow = NinjectWebCommon.GlobalKernal.Get<IUnitOfWork>();

                                   IQueryable<Account> query = uow.Accounts.Get().Where(m => m.AccountType_Id != 5);


                                   var search = options.GetAdditionalQueryOptionString("search");
                                   if (!string.IsNullOrEmpty(search))
                                   {
                                       search = search.Trim().ToLower();

                                       if (search.ToLower().Contains("not"))
                                       {
                                           query = query.Where(m => m.AccountType_Id == 1);

                                       }
                                       else if (search.ToLower().Contains("doc"))
                                       {
                                           query = query.Where(m => m.AccountType_Id == 2);

                                       }
                                       else if (search.ToLower().Contains("rej"))
                                       {
                                           query = query.Where(m => m.AccountType_Id == 3);

                                       }
                                       else if (search.ToLower().Contains("ver"))
                                       {
                                           query = query.Where(m => m.AccountType_Id == 4);

                                       }
                                       else
                                       {
                                           query = query.Where(m => m.FirstName.ToLower().StartsWith(search) || m.LastName.ToString().ToLower().StartsWith(search) || m.Country.ToString().ToLower().Contains(search) || m.Email.ToString().ToLower().Contains(search) || m.RefferenceNumber.ToLower().Contains(search));

                                       }

                                   }

                                   if (!string.IsNullOrEmpty(options.SortColumnName))
                                   {
                                       string sortDirection = (options.SortDirection == SortDirection.Dsc) ? "Desc" : options.SortDirection.ToString();
                                       query = query.OrderBy(options.SortColumnName + " " + sortDirection);
                                   }
                                   else
                                   {
                                       query = query.OrderByDescending(m => m.AccountId);
                                   }
                                   result.TotalRecords = query.Count();
                                   if (options.GetLimitOffset().HasValue)
                                   {
                                       query = query.Skip(options.GetLimitOffset().Value).Take(options.GetLimitRowcount().Value);
                                   }
                                   result.Items = query.ToList();


                                   return result;
                               })
              );
                #endregion 

            #region AccountsProfit
                MVCGridDefinitionTable.Add("AccountsProfitGrid", new MVCGridBuilder<Account>(MVCGridConfig.GridDefaults, MVCGridConfig.ColumnDefaults)
                 .AddColumns(c =>
                  {
                      //c.Add("CreationDate").WithValueExpression(m=>m.CreationDate.Value.ToString("dd MMMM, yyyy")).WithHeaderText("CreationDate").WithAllowChangeVisibility(true).WithFiltering(true);
                      c.Add("RefferenceNumber").WithValueExpression(m =>
                      {
                          string msg = "";
                          msg += "<span class='btn'><a style='color:#000' href='/Admin/AccountDetail?acId=" + m.AccountId + "'>" + m.RefferenceNumber + "</a></span>";
                          return msg;
                      }).WithHeaderText("RefNumber").WithHtmlEncoding(false).WithAllowChangeVisibility(true).WithFiltering(true).WithSorting(false);
                      c.Add("FirstName").WithValueExpression(m =>
                      {
                          string msg = "";
                          msg += "<span class='btn'><a style='color:#000' href='/Admin/AccountDetail?acId=" + m.AccountId + "'>" + m.FirstName + "</a></span>";
                          return msg;
                      }).WithHeaderText("FirstName").WithHtmlEncoding(false).WithAllowChangeVisibility(true).WithFiltering(true).WithSorting(true);
                      c.Add("LastName").WithValueExpression(m =>
                      {
                          string msg = "";
                          msg += "<span class='btn'><a style='color:#000' href='/Admin/AccountDetail?acId=" + m.AccountId + "'>" + m.LastName + "</a></span>";
                          return msg;
                      }).WithHeaderText("LastName").WithHtmlEncoding(false).WithAllowChangeVisibility(true).WithFiltering(true).WithSorting(true);
                      c.Add("Email").WithValueExpression(m =>
                      {
                          string msg = "";
                          msg += "<span class='btn'><a style='color:#000' href='/Admin/AccountDetail?acId=" + m.AccountId + "'>" + m.Email + "</a></span>";
                          return msg;
                      }).WithHeaderText("Email").WithHtmlEncoding(false).WithAllowChangeVisibility(true).WithFiltering(true).WithSorting(true);
                      c.Add("Password").WithValueExpression(m => m.UserRoles.First().Password).WithHeaderText("Password").WithAllowChangeVisibility(false).WithFiltering(false);
                      //c.Add("DOBDay").WithValueExpression(m => (m.DOBDay == null) ? "" : m.DOBYear + "-" + m.DOBMonth + "-" + m.DOBDay).WithHeaderText("Date of Birth").WithAllowChangeVisibility(true).WithFiltering(true).WithSorting(false);
                      c.Add("Status").WithValueExpression(m =>
                      {
                          string msg = "";
                          if (m.AccountType_Id == (int)AccountTypes.NEW)
                          {
                              msg += "<span class='btn-danger btn' style='width:126px'><a style='color:#fff' href='/Admin/AccountDetail?acId=" + m.AccountId + "'> NOT VERIFIED </a></span>";
                          }
                          else if (m.AccountType_Id == (int)AccountTypes.BASIC)
                          {
                              msg += "<span class='btn-warning btn' style='width:145px'><a style='color:#fff' href='/Admin/AccountDetail?acId=" + m.AccountId + "'> DOCS UPLOADED </a> </span>";
                          }
                          else if (m.AccountType_Id == (int)AccountTypes.REJECTED)
                          {
                              msg += "<span class='btn btn-danger' style='width:126px'><a style='color:#fff' href='/Admin/AccountDetail?acId=" + m.AccountId + "'> REJECTED </a> </span>";
                          }
                          else if (m.AccountType_Id == (int)AccountTypes.VERIFIED)
                          {
                              msg += "<span class='btn-success btn' style='width:126px'><a style='color:#fff' href='/Admin/AccountDetail?acId=" + m.AccountId + "'> VERIFIED </a></span>";
                          }
                          return msg;
                      }).WithHeaderText("Status").WithHtmlEncoding(false).WithAllowChangeVisibility(true).WithFiltering(true).WithSorting(false);

                      c.Add("Options").WithValueExpression(m =>
                      {
                          string html = @"<span>";
                          if (m.AccountType_Id == (int)AccountTypes.BASIC)
                          {
                              html += @"<a href=""/Admin/AccountDetail?acId=" + m.AccountId + @""" title=""Documents uploaded. View details"" class=""btn btn-sm btn-success""><i class=""fa fa-user""></i></a>";
                          }
                          else if (m.AccountType_Id == (int)AccountTypes.REJECTED)
                          {
                              html += @"<a href=""/Admin/AccountDetail?acId=" + m.AccountId + @""" title=""Documents rejected. View details"" class=""btn btn-sm btn-danger""><i class=""fa fa-user""></i></a>";
                          }
                          else
                          {
                              html += @"<a href=""/Admin/AccountDetail?acId=" + m.AccountId + @""" title=""View details"" class=""btn btn-sm btn-default""><i class=""fa fa-user""></i></a>";
                          }
                          if (m.AccountEnabled == true)
                          {
                              html += @" | <a href=""/Admin/SuspendAccount?acId=" + m.AccountId + @""" title=""suspend"" class=""btn btn-sm btn-default suspendAccount""><i class=""fa fa-lock""></i></a>";
                          }
                          else
                          {
                              html += @" | <a href=""/Admin/ActivateAccount?acId=" + m.AccountId + @""" title=""activate"" class=""btn btn-sm btn-default activateAccount""><i class=""fa fa-unlock""></i></a>";
                          }

                          html += "</span>";
                          return html;

                      }).WithFiltering(false).WithSorting(false).WithHtmlEncoding(false);
                  }).WithDefaultSortColumn("AccountId").WithDefaultSortDirection(SortDirection.Dsc).WithAdditionalQueryOptionName("search")
                    .WithRetrieveDataMethod((context) =>
                        {
                            var result  = new QueryResult<Account>();
                            var options = context.QueryOptions;
                            IUnitOfWork uow = NinjectWebCommon.GlobalKernal.Get<IUnitOfWork>();

                            List<Account> list    = uow.Accounts.Get().Where(m => m.AccountType_Id != 5).ToList();
                            List<Wallet>  wallets = uow.Accounts.GetWallet().Where(m => m.Currency == "USD").ToList();

                            List<Account> filterList = new List<Account>();

                            for (int t = 0; t < list.ToList().Count; t++)
                            {
                                var w = wallets.Where(x => x.Account_Id == list.ToList()[t].AccountId && x.Currency == "USD" && x.Balance > (decimal)10.0).FirstOrDefault();
                                if (w != null)
                                {
                                    if (w.Balance.Value > (decimal)10.0)
                                    {
                                        filterList.Add(list.ToList()[t]);
                                    }
                                }
                            }
                            IQueryable<Account> query = filterList.AsQueryable();
                            var search = options.GetAdditionalQueryOptionString("search");
                            if (!string.IsNullOrEmpty(search))
                            {
                                search = search.Trim().ToLower();

                                if (search.ToLower().Contains("not"))
                                {
                                    query = query.Where(m => m.AccountType_Id == 1);

                                }
                                else if (search.ToLower().Contains("doc"))
                                {
                                    query = query.Where(m => m.AccountType_Id == 2);

                                }
                                else if (search.ToLower().Contains("rej"))
                                {
                                    query = query.Where(m => m.AccountType_Id == 3);

                                }
                                else if (search.ToLower().Contains("ver"))
                                {
                                    query = query.Where(m => m.AccountType_Id == 4);

                                }
                                else
                                {
                                    query = query.Where(m => m.FirstName.ToLower().StartsWith(search) || m.LastName.ToString().ToLower().StartsWith(search) || m.Country.ToString().ToLower().Contains(search) || m.Email.ToString().ToLower().Contains(search) || m.RefferenceNumber.ToLower().Contains(search));

                                }

                            }

                            if (!string.IsNullOrEmpty(options.SortColumnName))
                            {
                                string sortDirection = (options.SortDirection == SortDirection.Dsc) ? "Desc" : options.SortDirection.ToString();
                                query = query.OrderBy(options.SortColumnName + " " + sortDirection);
                            }
                            else
                            {
                                query = query.OrderByDescending(m => m.AccountId);
                            }
                            result.TotalRecords = query.Count();
                            if (options.GetLimitOffset().HasValue)
                            {
                                query = query.Skip(options.GetLimitOffset().Value).Take(options.GetLimitRowcount().Value);
                            }
                            result.Items = query.ToList();


                            return result;
                        })
              );
                #endregion  
            
            #region AccountsLoss
                MVCGridDefinitionTable.Add("AccountsLossGrid", new MVCGridBuilder<Account>(MVCGridConfig.GridDefaults, MVCGridConfig.ColumnDefaults)
                 .AddColumns(c =>
                  {
                      //c.Add("CreationDate").WithValueExpression(m=>m.CreationDate.Value.ToString("dd MMMM, yyyy")).WithHeaderText("CreationDate").WithAllowChangeVisibility(true).WithFiltering(true);
                      c.Add("RefferenceNumber").WithValueExpression(m =>
                      {
                          string msg = "";
                          msg += "<span class='btn'><a style='color:#000' href='/Admin/AccountDetail?acId=" + m.AccountId + "'>" + m.RefferenceNumber + "</a></span>";
                          return msg;
                      }).WithHeaderText("RefNumber").WithHtmlEncoding(false).WithAllowChangeVisibility(true).WithFiltering(true).WithSorting(false);
                      c.Add("FirstName").WithValueExpression(m =>
                      {
                          string msg = "";
                          msg += "<span class='btn'><a style='color:#000' href='/Admin/AccountDetail?acId=" + m.AccountId + "'>" + m.FirstName + "</a></span>";
                          return msg;
                      }).WithHeaderText("FirstName").WithHtmlEncoding(false).WithAllowChangeVisibility(true).WithFiltering(true).WithSorting(true);
                      c.Add("LastName").WithValueExpression(m =>
                      {
                          string msg = "";
                          msg += "<span class='btn'><a style='color:#000' href='/Admin/AccountDetail?acId=" + m.AccountId + "'>" + m.LastName + "</a></span>";
                          return msg;
                      }).WithHeaderText("LastName").WithHtmlEncoding(false).WithAllowChangeVisibility(true).WithFiltering(true).WithSorting(true);
                      c.Add("Email").WithValueExpression(m =>
                      {
                          string msg = "";
                          msg += "<span class='btn'><a style='color:#000' href='/Admin/AccountDetail?acId=" + m.AccountId + "'>" + m.Email + "</a></span>";
                          return msg;
                      }).WithHeaderText("Email").WithHtmlEncoding(false).WithAllowChangeVisibility(true).WithFiltering(true).WithSorting(true);
                      c.Add("Password").WithValueExpression(m => m.UserRoles.First().Password).WithHeaderText("Password").WithAllowChangeVisibility(false).WithFiltering(false);
                      //c.Add("DOBDay").WithValueExpression(m => (m.DOBDay == null) ? "" : m.DOBYear + "-" + m.DOBMonth + "-" + m.DOBDay).WithHeaderText("Date of Birth").WithAllowChangeVisibility(true).WithFiltering(true).WithSorting(false);
                      c.Add("Status").WithValueExpression(m =>
                      {
                          string msg = "";
                          if (m.AccountType_Id == (int)AccountTypes.NEW)
                          {
                              msg += "<span class='btn-danger btn' style='width:126px'><a style='color:#fff' href='/Admin/AccountDetail?acId=" + m.AccountId + "'> NOT VERIFIED </a></span>";
                          }
                          else if (m.AccountType_Id == (int)AccountTypes.BASIC)
                          {
                              msg += "<span class='btn-warning btn' style='width:145px'><a style='color:#fff' href='/Admin/AccountDetail?acId=" + m.AccountId + "'> DOCS UPLOADED </a> </span>";
                          }
                          else if (m.AccountType_Id == (int)AccountTypes.REJECTED)
                          {
                              msg += "<span class='btn btn-danger' style='width:126px'><a style='color:#fff' href='/Admin/AccountDetail?acId=" + m.AccountId + "'> REJECTED </a> </span>";

                          }
                          else if (m.AccountType_Id == (int)AccountTypes.VERIFIED)
                          {
                              msg += "<span class='btn-success btn' style='width:126px'><a style='color:#fff' href='/Admin/AccountDetail?acId=" + m.AccountId + "'> VERIFIED </a></span>";

                          }
                          return msg;
                      }).WithHeaderText("Status").WithHtmlEncoding(false).WithAllowChangeVisibility(true).WithFiltering(true).WithSorting(false);

                      c.Add("Options").WithValueExpression(m =>
                      {

                          string html = @"<span>";
                          if (m.AccountType_Id == (int)AccountTypes.BASIC)
                          {
                              html += @"<a href=""/Admin/AccountDetail?acId=" + m.AccountId + @""" title=""Documents uploaded. View details"" class=""btn btn-sm btn-success""><i class=""fa fa-user""></i></a>";
                          }
                          else if (m.AccountType_Id == (int)AccountTypes.REJECTED)
                          {
                              html += @"<a href=""/Admin/AccountDetail?acId=" + m.AccountId + @""" title=""Documents rejected. View details"" class=""btn btn-sm btn-danger""><i class=""fa fa-user""></i></a>";
                          }
                          else
                          {
                              html += @"<a href=""/Admin/AccountDetail?acId=" + m.AccountId + @""" title=""View details"" class=""btn btn-sm btn-default""><i class=""fa fa-user""></i></a>";

                          }
                          if (m.AccountEnabled == true)
                          {
                              html += @" | <a href=""/Admin/SuspendAccount?acId=" + m.AccountId + @""" title=""suspend"" class=""btn btn-sm btn-default suspendAccount""><i class=""fa fa-lock""></i></a>";
                          }
                          else
                          {
                              html += @" | <a href=""/Admin/ActivateAccount?acId=" + m.AccountId + @""" title=""activate"" class=""btn btn-sm btn-default activateAccount""><i class=""fa fa-unlock""></i></a>";

                          }

                          html += "</span>";
                          return html;

                      }).WithFiltering(false).WithSorting(false).WithHtmlEncoding(false);

                  }).WithDefaultSortColumn("AccountId").WithDefaultSortDirection(SortDirection.Dsc).WithAdditionalQueryOptionName("search")
                 .WithRetrieveDataMethod((context) =>
                               {

                                   var result = new QueryResult<Account>();
                                   var options = context.QueryOptions;
                                   IUnitOfWork uow = NinjectWebCommon.GlobalKernal.Get<IUnitOfWork>();

                                   List<Account> list = uow.Accounts.Get().Where(m => m.AccountType_Id != 5).ToList();
                                   List<Wallet> wallets = uow.Accounts.GetWallet().Where(m => m.Currency == "USD").ToList();

                                   List<Account> filterList = new List<Account>();

                                   for (int t = 0; t < list.ToList().Count; t++)
                                   {
                                       var w = wallets.Where(x => x.Account_Id == list.ToList()[t].AccountId && x.Currency == "USD" && x.Balance <= (decimal)10.0).FirstOrDefault();
                                       if (w != null)
                                       {
                                           if (w.Balance.Value <= (decimal)10.0)
                                           {
                                               filterList.Add(list.ToList()[t]);
                                           }
                                       }
                                   }
                                   IQueryable<Account> query = filterList.AsQueryable();

                                   var search = options.GetAdditionalQueryOptionString("search");
                                   if (!string.IsNullOrEmpty(search))
                                   {
                                       search = search.Trim().ToLower();

                                       if (search.ToLower().Contains("not"))
                                       {
                                           query = query.Where(m => m.AccountType_Id == 1);

                                       }
                                       else if (search.ToLower().Contains("doc"))
                                       {
                                           query = query.Where(m => m.AccountType_Id == 2);

                                       }
                                       else if (search.ToLower().Contains("rej"))
                                       {
                                           query = query.Where(m => m.AccountType_Id == 3);

                                       }
                                       else if (search.ToLower().Contains("ver"))
                                       {
                                           query = query.Where(m => m.AccountType_Id == 4);

                                       }
                                       else
                                       {
                                           query = query.Where(m => m.FirstName.ToLower().StartsWith(search) || m.LastName.ToString().ToLower().StartsWith(search) || m.Country.ToString().ToLower().Contains(search) || m.Email.ToString().ToLower().Contains(search) || m.RefferenceNumber.ToLower().Contains(search));

                                       }

                                   }

                                   if (!string.IsNullOrEmpty(options.SortColumnName))
                                   {
                                       string sortDirection = (options.SortDirection == SortDirection.Dsc) ? "Desc" : options.SortDirection.ToString();
                                       query = query.OrderBy(options.SortColumnName + " " + sortDirection);
                                   }
                                   else
                                   {
                                       query = query.OrderByDescending(m => m.AccountId);
                                   }
                                   result.TotalRecords = query.Count();
                                   if (options.GetLimitOffset().HasValue)
                                   {
                                       query = query.Skip(options.GetLimitOffset().Value).Take(options.GetLimitRowcount().Value);
                                   }
                                   result.Items = query.ToList();


                                   return result;
                               })
              );
                #endregion

                #region AdminAccounts
                MVCGridDefinitionTable.Add("AdminAccountsGrid", new MVCGridBuilder<Account>(MVCGridConfig.GridDefaults, MVCGridConfig.ColumnDefaults)

                  .AddColumns(c =>
                  {
                      c.Add("FirstName").WithValueExpression(m => m.FirstName).WithHeaderText("First Name").WithAllowChangeVisibility(true).WithFiltering(true);
                      c.Add("LastName").WithValueExpression(m => m.LastName).WithHeaderText("Last Name").WithAllowChangeVisibility(true).WithFiltering(true);
                      c.Add("Email").WithValueExpression(m => m.Email).WithHeaderText("Email").WithAllowChangeVisibility(true).WithFiltering(true);
                      c.Add("UserRoles.FirstOrDefault.UserRoleType.UserRoles").WithValueExpression(m => m.UserRoles.FirstOrDefault().UserRoleType.RoleType.ToString()).WithHeaderText("User Type").WithAllowChangeVisibility(true).WithFiltering(false);

                      c.Add("Status").WithValueExpression(m =>
                      {
                          string msg = "";
                          if (m.AccountEnabled == false)
                          {
                              msg += "<span class='btn btn-sm btn-danger'>DEACTIVATED</span>";
                          }
                          else
                          {
                              msg += "<span class='btn btn-sm btn-success'>ACTIVATED</span>";
                          }
                          return msg;
                      }).WithHeaderText("Status").WithHtmlEncoding(false).WithAllowChangeVisibility(true).WithFiltering(true).WithSorting(false);
                      c.Add("Options").WithValueExpression(m =>
                      {

                          string html = @"<span>";
                          //if (m.AccountType_Id == (int)AccountTypes.BASIC)
                          //{
                          //    html += @"<a href=""/Admin/AccountDetail?acId=" + m.AccountId + @""" title=""Documents uploaded. View details"" class=""btn btn-sm btn-success""><i class=""fa fa-user""></i></a>";
                          //}
                          //else if (m.AccountType_Id == (int)AccountTypes.REJECTED)
                          //{
                          //    html += @"<a href=""/Admin/AccountDetail?acId=" + m.AccountId + @""" title=""Documents rejected. View details"" class=""btn btn-sm btn-danger""><i class=""fa fa-user""></i></a>";
                          //}
                          //else
                          //{
                          //    html += @"<a href=""/Admin/AccountDetail?acId=" + m.AccountId + @""" title=""View details"" class=""btn btn-sm btn-default""><i class=""fa fa-user""></i></a>";

                          //}
                          

                          if (m.AccountEnabled == true)
                          {
                              html += @" | <a href=""/Admin/SuspendAdminAccount?acId=" + m.AccountId + @""" title=""suspend"" class=""btn btn-sm btn-default suspendAdmin""><i class=""fa fa-lock""></i></a>";
                          }
                          else
                          {
                              html += @" | <a href=""/Admin/ActivateAdminAccount?acId=" + m.AccountId + @""" title=""activate"" class=""btn btn-sm btn-default activateAdmin""><i class=""fa fa-unlock""></i></a>";

                          }

                          html += "</span>";
                          return html;

                      }).WithFiltering(false).WithSorting(false).WithHtmlEncoding(false);
                  }).WithDefaultSortColumn("AccountId").WithDefaultSortDirection(SortDirection.Dsc).WithAdditionalQueryOptionName("search")
                 .WithRetrieveDataMethod((context) =>
                 {
                     var result = new QueryResult<Account>();
                     var options = context.QueryOptions;
                     IUnitOfWork uow = NinjectWebCommon.GlobalKernal.Get<IUnitOfWork>();

                     var ac = SessionItems.Get(SessionKey.ACCOUNT) as Account;

                     IQueryable<Account> query = uow.Accounts.Get().Where(m => m.AccountType_Id == 5 && m.AccountId != ac.AccountId);


                     var search = options.GetAdditionalQueryOptionString("search");
                     if (!string.IsNullOrEmpty(search))
                     {
                         search = search.Trim().ToLower();
                         query = query.Where(m => m.FirstName.ToLower().StartsWith(search) || m.LastName.ToString().ToLower().Contains(search) || m.UserRoles.FirstOrDefault().UserRoleType.RoleType.ToString().ToLower().Contains(search) || m.Email.ToString().ToLower().Contains(search));
                     }
                     if (!string.IsNullOrEmpty(options.SortColumnName))
                     {
                         string sortDirection = (options.SortDirection == SortDirection.Dsc) ? "Desc" : options.SortDirection.ToString();
                         query = query.OrderBy(options.SortColumnName + " " + sortDirection);
                     }
                     else
                     {
                         query = query.OrderByDescending(m => m.AccountId);
                     }
                     result.TotalRecords = query.Count();
                     if (options.GetLimitOffset().HasValue)
                     {
                         query = query.Skip(options.GetLimitOffset().Value).Take(options.GetLimitRowcount().Value);
                     }
                     result.Items = query.ToList();


                     return result;
                 })
              );
                #endregion

            }
        }
}