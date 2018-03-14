using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAccess.Dao;
using DataAccess.Models;

namespace LofovaChyse.Areas.Admin.Controllers
{
    public class UsersController : Controller
    {
        // GET: Admin/Users
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GenUserRoles()
        {
            // Vezmeme všechny uživatele
            KnihovnaUserDao d = new KnihovnaUserDao();
            KnihovnaUserRoleDao dd = new KnihovnaUserRoleDao();

            List<KnihovnaUser> users = d.GetAll() as List<KnihovnaUser>;

            foreach (KnihovnaUser u in users)
            {
                KnihovnaUserRole r = new KnihovnaUserRole();
                r.Id = Books.Counter();
                r.Data = 0;
                r.DateFrom = DateTime.Now;
                r.DateTo = DateTime.MinValue;
                r.RoleId = 2;
                r.UserId = u.Id;

                dd.Create(r);
            }

            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult GiveUserRole()
        {
            KnihovnaRoleDao bookCategoryDao = new KnihovnaRoleDao();
            IList<KnihovnaRole> categories = bookCategoryDao.GetAll();
            ViewBag.Categories = categories;

            return View();
        }

        public ActionResult AddRole(KnihovnaUserRole role, int roleId)
        {
            KnihovnaUserRoleDao d = new KnihovnaUserRoleDao();
            role.Id = Books.Counter();
            role.DateFrom = DateTime.Now;
            role.RoleId = roleId;

            d.Create(role);

            return Redirect(Request.UrlReferrer.ToString());
        }
    }
}