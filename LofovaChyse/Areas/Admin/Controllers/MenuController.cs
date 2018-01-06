using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAccess.Dao;
using DataAccess.Models;

namespace LofovaChyse.Areas.Admin.Controllers
{
    [Authorize]
    public class MenuController : Controller
    {
        // GET: Admin/Menu
        [ChildActionOnly] // Může být zavolaný pouze jinou akcí
        public ActionResult Index()
        {
            KnihovnaUserDao knihovna = new KnihovnaUserDao();
            KnihovnaUser user = knihovna.GetByLogin(User.Identity.Name);

            return View(user);
        }
    }
}