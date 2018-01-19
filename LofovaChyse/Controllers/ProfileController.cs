using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAccess.Dao;
using DataAccess.Models;

namespace LofovaChyse.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
        [Authorize]
        public ActionResult Index()
        {
            KnihovnaUserDao knihovnaUserDao = new KnihovnaUserDao();
            KnihovnaUser knihovnaUser = knihovnaUserDao.GetByLogin(User.Identity.Name);


            return View(knihovnaUser);
        }
    }
}