using Exchange.Common;
using Exchange.DTO;
using Exchange.EF;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Exchange.UI
{
    public class ExchangeAuthorizeAttribute : ActionFilterAttribute
    {

        //public override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Origin", "https://api.binance.com");
        //    base.OnActionExecuting(filterContext);
        //}


        //public override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    var date = DateTime.Parse("1/10/2020");

        //    ViewResult view = new ViewResult();
        //    view.ViewName = "~/Views/Dashboard/Index.cshtml";
        //    if (SessionItems.Get(SessionKey.ACCOUNT) == null)
        //    {
        //        SessionItems.Add(SessionKey.PREV_URL, filterContext.HttpContext.Request.RawUrl);
        //        //view.ViewBag.error = "Please login to continue";
        //        filterContext.Result = view;
        //    }
        //   else if (DateTime.Now > date)
        //    {
        //        SessionItems.RemoveAll();
        //        view.ViewBag.error = "Unknown Error";
        //        filterContext.Result = view;
        //    }
        //    else
        //    {
        //        var _ac = SessionItems.Get(SessionKey.ACCOUNT) as Account;
        //        using (ExchangeEntities _db = new ExchangeEntities())
        //        {
        //            _ac = _db.Accounts.Where(x => x.AccountId == _ac.AccountId).FirstOrDefault();
        //        }
        //        if (_ac.AccountEnabled == false)
        //        {
        //            SessionItems.RemoveAll();
        //            view.ViewBag.error = "Invalid Account";
        //            filterContext.Result = view;
        //        }
        //    }
        //    base.OnActionExecuting(filterContext);
        //}

    }
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class NoCacheAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            filterContext.HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            filterContext.HttpContext.Response.Cache.SetValidUntilExpires(false);
            filterContext.HttpContext.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            filterContext.HttpContext.Response.Cache.SetNoStore();

            base.OnResultExecuting(filterContext);
        }
    }
}