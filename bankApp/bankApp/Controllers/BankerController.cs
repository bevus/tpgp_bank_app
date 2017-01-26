using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace bankApp.Controllers
{
    public class BankerController : Controller
    {
        // GET: Banker
        public ActionResult Index()
        {
            return View();
        }
    }
}