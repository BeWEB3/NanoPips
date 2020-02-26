using Exchange.Common;
using Exchange.DTO;
using Exchange.UI.Models;
using Exchange.UOW;
using QRCoder;
using System;
using System.Drawing;
using System.IO;
using System.Web.Mvc;

namespace Exchange.UI.Controllers
{
    public class AccountController : Controller
    {
        private IUnitOfWork _uow;

        public AccountController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public ActionResult Login(string code = null, string email = null)
        {
            if (code != null)
            {
                ViewBag.code = code;
            }
            if(email != null)
            {
                ViewBag.email = email;
            }
            return View();
        }

        public ActionResult landing()
        {
            return View();
        }

        public ActionResult contact()
        {
            return View();
        }

        public ActionResult terms()
        {
            return View();
        }

        public ActionResult pricing()
        {
            return View();
        }

        public ActionResult rate()
        {
            return View();
        }

        public ActionResult funding()
        {
            return View();
        }

        public ActionResult LandingPage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserRole model, double utcTime = 0)
        {
            try
            {
                var ac = _uow.Accounts.Login(model.Username, model.Password);
                if (ac.AccountEnabled == false)
                {
                    ViewBag.error = "Your account is disabled, please contact support for further assistance";
                    return View();
                }
                if (ac.AccountType_Id == (int) AccountTypes.ADMIN)
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
                ViewBag.error = ex.ErrorMessage;
            }
            return View();
        }

        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public string SignUp(UserRole model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var ac = _uow.Accounts.SignUp(model.Username.ToLower().Trim(), model.Password, model.RefferenceNumber);
                    _uow.Accounts.SendVerificationEmail(ac.AccountId);
                    return "<b>Registration Step-2 Verify Email Address</b>" +
                        " <br /><br /> A Verification email has been sent to <b>" + model.Username + "</b>. Please click the email link to confirm & set up your account" +
                        " <br /><br /> No email?- Check spam folders, or 'register' again (Maybe there was a typo last time)";
                }
                catch (ExchangeException ex)
                {
                    return ex.Message.Replace("_", " ").ToLower();
                }
            }
            else return "Please fill the required fields first";
        }

        [HttpPost]
        public string SignUpLandingPage(UserRole model)
        {
            //if (ModelState.IsValid)
            //{
                try
                {
                    var ac = _uow.Accounts.SignUp(model.Username.ToLower().Trim(), model.Password, model.RefferenceNumber);
                    _uow.Accounts.SendVerificationEmail(ac.AccountId);
                return "OK";
                //return RedirectToAction("Index", "Dashboard");
                }
                catch (ExchangeException ex)
                {
                return ex.Message.Replace("_", " ").ToLower();
                //return RedirectToAction("LandingPage", "Account");
            }
            //}
           //return "Please fill the required fields first";
            //else return RedirectToAction("LandingPage", "Account");
        }

        public bool CheckEmail(string email)
        {
            return _uow.Accounts.CheckEmail(email.Trim().ToLower());
        }

        public ActionResult Referral() {
            var acc = SessionItems.Get(SessionKey.ACCOUNT) as Account;
            RefferalData data = _uow.Accounts.GetRefferalData(acc.AccountId);
            return View(data);
        }

        public ActionResult GetQrCode(string code)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(code, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            var bytes = BitmapToBytes(qrCodeImage);
            var barCode = Convert.ToBase64String(bytes);
            var obj = new
            {
                barCode
            };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        private static byte[] BitmapToBytes(Bitmap img)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }

        public string VerifyEmail(string code)
        {
            try
            {
                _uow.Accounts.VerifyEmail(code);
                return "Email address has been verfied successfully, please login to continue";
            }
            catch (ExchangeException ex)
            {
                return ex.ErrorMessage;
            }
        }

        public string VerifyEmailForIPAgent(string code, string email)
        {
            try
            {
                _uow.Accounts.VerifyEmail(code, email);
                return "Email address has been verfied successfully, please login to continue";
            }
            catch (ExchangeException ex)
            {
                return ex.ErrorMessage;
            }
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public string ForgotPassword(string email)
        {
            try
            {
                _uow.Accounts.SendForgotPasswordEmail(email.Trim().ToLower());
                return "An email with password reset instructions was sent to your email address.";
            }
            catch (ExchangeException ex)
            {
                return ex.ErrorMessage;
            }
        } 

        public ActionResult VerifyPasswordReset(string code)
        {
            try
            {
                var ac = _uow.Accounts.VerifyPasswordReset(code);
                return View(new UserRole() { Username = ac.Email, Account_Id = ac.AccountId });
            }
            catch (ExchangeException ex)
            {
                ViewBag.error = ex.ErrorMessage;
                return View();
            }
        
        }

        public ActionResult SendEmail(string name, string email, string phone, string subject, string message)
        {
            var res = _uow.Accounts.SendEmail(name, email, phone, subject, message);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string ResetPassword(UserRole model)
        {
            try
            {
                _uow.Accounts.ResetPassword(model.Account_Id.Value, model.Password);
                _uow.Accounts.SaveUserActivity(Request.UserAgent, Request.UserHostAddress, model.Username, "Password Reset");
                return "Password has been updated successfully. Please login to continue.";
            }
            catch (ExchangeException ex)
            {
                return ex.ErrorMessage;
            }
        }

        [ExchangeAuthorize]
        public ActionResult _header()
        {
            var ac = SessionItems.Get(SessionKey.ACCOUNT) as Account;
            ViewBag.email = ac.Email;
            return View();
        }

        public ActionResult Logout()
        {
            SessionItems.RemoveAll();
            return RedirectToAction("Index", "Dashboard");
        }

        public ActionResult Help()
        {
            return View();
        }

        public ActionResult Profile(bool showPopUp = false)
        {
            ViewBag.ShowPopUp = showPopUp;
            var acc = SessionItems.Get(SessionKey.ACCOUNT) as Account;
            var ProfileModel = new ProfileModel
            {
                DepositHistory  = _uow.Accounts.GetDepositHistory(acc.AccountId),
                WithdrawHistory = _uow.Accounts.GetWithdrawHistory(acc.AccountId),
                ChangePassword  = new ChangePassword(),
                TradeHistory    = _uow.Accounts.GetTradeHistory(acc.AccountId),
                CurrencyList    = _uow.Accounts.GetCurrencyList(),
                KYC             = new KYC(),
                securityAnswer  = new securityModel(),
                Payment         = new Payment(),
                GetAccount      = _uow.Accounts.GetAccount(acc.AccountId)
            };
            return View(ProfileModel);
        }

        public ActionResult ChangePassword(string oldPassword, string newPassword)
        {
            try
            {
                var _ac = SessionItems.Get(SessionKey.ACCOUNT) as Account;
                var ac  = _uow.Accounts.GetAccount(_ac.AccountId);
                bool v = _uow.Accounts.ChangePassword(ac.AccountId, oldPassword, newPassword);

                var obj = new {
                    message = "Password has been updated successfully",
                    status  = true
                };
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            catch (ExchangeException ex)
            {
                var obj = new
                {
                    message = "An error occured! " + ex.ErrorMessage,
                    status = false
                };
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult KYC(KYC model)
        {
            try
            {
                var _ac = SessionItems.Get(SessionKey.ACCOUNT) as Account;
                var ac  = _uow.Accounts.GetAccount(_ac.AccountId);
                if (Request.Files.Count > 0)
                {
                    var file = Request.Files[0];
                    model.FirstDocument = "/Assets/Proof/" + DateTime.Now.Ticks.ToString() + file.FileName;
                    file.SaveAs(Server.MapPath(model.FirstDocument));
                    var file2 = Request.Files[1];
                    model.SecondDocument = "/Assets/Proof/" + DateTime.Now.Ticks.ToString() + file2.FileName;
                    file2.SaveAs(Server.MapPath(model.SecondDocument));
                }
                var detail = _uow.Accounts.kyc(model, ac.AccountId, Server.MapPath(model.FirstDocument), Server.MapPath(model.SecondDocument));
                _uow.Accounts.UpdateAccount(ac.AccountId);
                TempData["msg"] = "Your profile and documents has been uploaded successfully. We are reviewing your documents and get back to you shortly.";

                return RedirectToAction("Index", "Dashboard");
            }
            catch (ExchangeException ex)
            {
                ViewBag.msg = ex.ErrorMessage;
                return RedirectToAction("Index", "Dashboard");
            }
        }

        [HttpPost]
        public ActionResult WireTransfer(ProfileModel model)
        {
            try
            {
                var _ac = SessionItems.Get(SessionKey.ACCOUNT) as Account;
                var ac  = _uow.Accounts.GetAccount(_ac.AccountId);
                var obj = new Payment() {
                    Account_Id = ac.AccountId,
                    FiatCurrency = "USD",
                    FiatAmount  = model.Payment.FiatAmount,
                    BankAddress = model.Payment.BankAddress,
                    PaymentType = "FIAT_WITHDRAW",
                    BankAccountTitle = model.Payment.BankAccountTitle,
                    BankName = model.Payment.BankName,
                    BankAccountNumber = model.Payment.BankAccountNumber,
                    PaymentDate = DateTime.Now,
                    IBAN  = model.Payment.IBAN,
                    BankCity = model.Payment.BankCity,
                    Reason = model.Payment.Reason,
                    Currency = "USD",
                    Source = "FIAT",
                    Amount = model.Payment.FiatAmount,
                    AmountSent = Convert.ToDecimal(model.Payment.FiatAmount),
                    PaymentStatus_Id = (int) 1,
                    StatusMessage = "Withdrawl Requested"
                };

                var account = _uow.Payment.WireWithDrawlReqAdmin(obj);
                SessionItems.Add(SessionKey.ACCOUNT, account);
                Session["msg"] = "Wire transfer has been successfully submitted to admin. Wait for confirmation";
                return RedirectToAction("Profile","Account");
            }
            catch (ExchangeException ex)
            {
                Session["msg"] = "Some error occured, try agin later";
                return RedirectToAction("Profile", "Account");
            }
        }

        public ActionResult Buy_Funds()
        {
            return View();
        }

        public ActionResult Log()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SecurityDetails(string firstQuestion,string SecAns1,string secondQuestion,string SecAns2,string thirdQuestion,string SecAns3)
        {
            try
            {
                var _ac = SessionItems.Get(SessionKey.ACCOUNT) as Account;
                var ac = _uow.Accounts.GetAccount(_ac.AccountId);
                if (firstQuestion != "-- select question --" && secondQuestion != "-- select question --" && thirdQuestion != "-- select question --" &&
                    SecAns1 != null && SecAns1 != "" && SecAns2 != null && SecAns2 != "" && SecAns3 != null && SecAns3 != "")
                {
                    var detail = _uow.Accounts.securityAnswer(ac.AccountId, firstQuestion, SecAns1, secondQuestion, SecAns2, thirdQuestion, SecAns3);
                }
                else
                {
                    ViewBag.msg = "Please fill all fields.";
                    return RedirectToAction("Index", "Dashboard");
                }
                return RedirectToAction("Profile", "Account");
            }
            catch (ExchangeException ex)
            {
                ViewBag.msg = ex.ErrorMessage;
                return RedirectToAction("Profile", "Account");
            }
        }

        public ActionResult TestChart() {
            return View();
        }
    }
}