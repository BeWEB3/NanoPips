using Exchange.Common;
using Exchange.DTO;
using Exchange.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Exchange.UI
{
    public class AdminAuthorizeAttribute : ActionFilterAttribute
    {
        ExchangeEntities db = new ExchangeEntities();
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            
            if (SessionItems.Get(SessionKey.ACCOUNT) == null && SessionItems.Get(SessionKey.ADMIN)==null)
            {
                ViewResult view = new ViewResult();
                view.ViewName = "~/Views/Account/Login.cshtml";
                view.ViewBag.error = "Please login to continue";
                filterContext.Result = view;

            }
            else
            {
                string controller = filterContext.Controller.GetType().Name.Replace("Controller", ""); 
                // actionContext.ControllerContext.Controller.GetType().Name.Replace("Controller", "");
                string function = filterContext.ActionDescriptor.ActionName;
               
                var rights = db.FunctionAccesses;
                var ac = SessionItems.Get(SessionKey.ACCOUNT) as Account;
                byte? userType = ac.UserRoles.FirstOrDefault().UserRoleTypeId;
                bool isAccessible = false;

                foreach (var item in rights)
                {
                    if(item.FunctionRequest.ControllerName == controller && item.FunctionRequest.FunctionName == function && item.UserRoleTypeId == userType && item.IsAccessible == true)
                    {
                        isAccessible = true;
                    }
                       
                }

                if (function.StartsWith("_"))
                {
                    isAccessible = true;
                }
                

                if (isAccessible == false)
                {
                    ViewResult view = new ViewResult();
                    view.ViewName = "~/Views/Account/Login.cshtml";
                    view.ViewBag.error = "You are not allowed to access this.";
                    filterContext.Result = view;
                }

            }

            base.OnActionExecuting(filterContext);
        }
    }
   
}