using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using DataAccess.Dao;
using DataAccess.Models;

namespace LofovaChyse.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
        [Authorize]
        public ActionResult Index(int id)
        {
            KnihovnaUserDao knihovnaUserDao = new KnihovnaUserDao();
            KnihovnaUser knihovnaUser = knihovnaUserDao.GetbyId(id);

            bool v = false;

            if (knihovnaUser.Name == knihovnaUserDao.GetByLogin(User.Identity.Name).Name)
            {
                v = true;
            }
            else
            {
                v = false;
            }

            ViewBag.Owner = v;
            return View(knihovnaUser);
        }

        [ValidateInput(false)]
        public ActionResult EditProfile(string welcomeText)
        {
            KnihovnaUserDao knihovnaUserDao = new KnihovnaUserDao();
            KnihovnaUser knihovnaUser = knihovnaUserDao.GetByLogin(User.Identity.Name);

            knihovnaUser.WelcomeText = welcomeText;

            knihovnaUserDao.Update(knihovnaUser);

            return RedirectToAction("Index", new {id = knihovnaUser.Id});
        }
    }
}