using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAccess.Dao;
using DataAccess.Models;

namespace LofovaChyse.Controllers
{
    public class NotifikaceController : Controller
    {
        // GET: Notifikace
        public ActionResult Index(int userId)
        {
            KnihovnaNotifikaceDao dao = new KnihovnaNotifikaceDao();
            IList<KnihovnaNotifikace> list = dao.GetUserNotifikace(userId);

            foreach (KnihovnaNotifikace n in list)
            {
                n.Displayed = true;
                dao.Update(n);
            }


            return View(list);
        }

        public JsonResult Dismiss(int notifikaceId)
        {
            KnihovnaUserDao d = new KnihovnaUserDao();
            KnihovnaUser u = d.GetByLogin(User.Identity.Name);

            KnihovnaNotifikaceDao dao = new KnihovnaNotifikaceDao();
            KnihovnaNotifikace n = dao.GetbyId(notifikaceId);

            if (n.RewardType == 1)
            {
                u.Money += 10;
            }

            d.Update(u);
            dao.Delete(n);
            TempData["Message"] = "Your Order";

            return Json(new { });
        }
    }
}