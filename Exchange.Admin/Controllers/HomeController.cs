using Exchange.Common;
using Exchange.DTO;
using Exchange.UOW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Exchange.UI.Controllers
{
    public class HomeController : Controller
    {
        private IUnitOfWork _uow;
        public HomeController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public ActionResult Index()
        {
            
            return View();
        }
    }
}