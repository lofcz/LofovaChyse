using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace LofovaChyse.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignIn(string login, string password)
        {
            if (Membership.ValidateUser(login, password))
            {
                FormsAuthentication.SetAuthCookie(login, false);
                return RedirectToAction("Index", "Home", new {area = ""});
            }

            TempData["error"] = "Přihlášení selhalo";
            return RedirectToAction("Index", "Login");


        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();

            return RedirectToAction("Index", "Login");
        }
    }
}