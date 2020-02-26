using Exchange.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Exchange.UI
{
    public class ExchangeAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            
            if (SessionItems.Get(SessionKey.ACCOUNT) == null)
            {
                ViewResult view = new ViewResult();
                view.ViewName = "~/Views/Account/Login.cshtml";
                view.ViewBag.error = "Please login to continue";
                filterContext.Result = view;

            } else if (SessionItems.Get(SessionKey.TWOFA) == null)
            {
                ViewResult view = new ViewResult();
                view.ViewName = "~/Views/Account/TwoFA.cshtml";
                view.ViewBag.error = "Please enter two factor code first";
                filterContext.Result = view;

            }
            base.OnActionExecuting(filterContext);
        }
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