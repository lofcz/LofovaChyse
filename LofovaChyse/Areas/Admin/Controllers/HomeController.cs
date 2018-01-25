using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace LofovaChyse.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        // GET: Admin/Home
        [Authorize(Roles = "knihovnik")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Test(int id)
        {
            ViewBag.Id = id;
            return View();
        }
    }
}