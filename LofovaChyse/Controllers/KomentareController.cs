using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAccess.Dao;
using DataAccess.Models;

namespace LofovaChyse.Controllers
{
    public class KomentareController : Controller
    {
        // GET: Komentare
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        [HttpPost, ValidateInput(false)]
        public ActionResult Add(string description, int topicId)
        {
            KnihovnaKomentareDao knihovnaKomentareDao = new KnihovnaKomentareDao();
            KnihovnaKomentare komentar = new KnihovnaKomentare();

            komentar.Id = Books.Counter();
            komentar.Content = description;
            komentar.OwnerId = new KnihovnaUserDao().GetByLogin(User.Identity.Name);
            komentar.Date = DateTime.Now;
            komentar.TopicId = topicId;

            knihovnaKomentareDao.Create(komentar);

            return Redirect(this.Request.UrlReferrer.AbsolutePath);
        }
    }
}