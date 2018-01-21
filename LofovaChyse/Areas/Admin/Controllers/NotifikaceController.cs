using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using DataAccess.Dao;
using DataAccess.Models;
using LofovaChyse.Class;

namespace LofovaChyse.Areas.Admin.Controllers
{
    public class NotifikaceController : Controller
    {
        // GET: Admin/Notifikace
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost] // post only
        [Authorize(Roles = "knihovnik")]
        [ValidateInput(false)]
        public ActionResult Add(string userTo, string text, bool sendIncognito)
        {
            if (ModelState.IsValid)
            {
                KnihovnaNotifikace n = new KnihovnaNotifikace()
                {
                    Text = text,
                    DateSent = DateTime.Now,
                    Displayed = false,
                    ForceType = 0,
                    Id = Books.Counter(),
                    RewardType = 0,
                    UserTo = int.Parse(userTo)
                };

                if (sendIncognito)
                {
                    n.UserFrom = -1;
                }
                else
                {
                    n.UserFrom = new KnihovnaUserDao().GetByLogin(User.Identity.Name).Id;
                }

                KnihovnaNotifikaceDao dao = new KnihovnaNotifikaceDao();
                dao.Create(n);

                // Notifikace
                TempData["scs"] = "V pořádku";
            }

            return RedirectToAction("Index");
        }
    }
}