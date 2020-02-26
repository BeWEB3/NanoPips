using Exchange.Common;
using Exchange.DTO;
using Exchange.UOW;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;
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
        public ActionResult _notification()
        {
            var ac = SessionItems.Get(SessionKey.ACCOUNT) as Account;
            return View(_uow.Accounts.GetNotifications().OrderByDescending(m=>m.NotificationId).Take(10).ToList());
        }
      
        public ActionResult Login(string code = null, string email = null)
        {
            if (code != null)
            {
                ViewBag.code = code;
            }
            if (email != null)
            {
                ViewBag.email = email;
            }
            return View();
        }
        [HttpPost]
        public ActionResult Login(UserRole model)
        {
            try
            {
                var ac = _uow.Accounts.Login(model.Username, model.Password);
                if (ac.AccountEnabled == false)
                {
                    TempData["error"] = "Your account is disabled, please contact support for further assistance";
                    return RedirectToAction("Login", "Account");
                }
                //var res = _uow.Accounts.CheckAgentAndHost(Request.UserAgent, Request.UserHostAddress, ac.Email, ac.AccountId);
                //if (res == false)
                //{
                //    ViewBag.error = "Browser or IP has changed, a verification email has been sent to you. Click the link to verify your account.";
                //    return View();
                //}
                SessionItems.Add(SessionKey.ACCOUNT, ac);
                //if (ac.TwoFactorEnabled == true)
                //{
                //    return RedirectToAction("TwoFA", "Account");
                //}
                if (ac.AccountType_Id == (int)AccountTypes.ADMIN)
                {
                    SessionItems.Add(SessionKey.ADMIN, true);
                    _uow.Accounts.SaveUserActivity(Request.UserAgent, Request.UserHostAddress, ac.Email, "Login");
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    TempData["error"] = "Invalid username or password";
                    return RedirectToAction("Login", "Account");

                }
                
            }
            catch (ExchangeException ex)
            {
                ViewBag.error = ex.ErrorMessage;
            }
            return View();
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
            return RedirectToAction("Index", "Admin");
        }
    }
}