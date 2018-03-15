using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAccess.Dao;
using DataAccess.Models;
using LofovaChyse.Class;

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
        public ActionResult Add(string description, int topicId, int reply, string type = "")
        {
            if (type != "create" && type != "")
            {
                Edit(int.Parse(type), description);
            }
            else
            {          
                KnihovnaKomentareDao knihovnaKomentareDao = new KnihovnaKomentareDao();
                KnihovnaKomentare komentar = new KnihovnaKomentare();

                komentar.Id = Books.Counter();
                komentar.Content = description;
                komentar.OwnerId = new KnihovnaUserDao().GetByLogin(User.Identity.Name);
                komentar.Date = DateTime.Now;
                komentar.TopicId = topicId;
                komentar.ReplyId = reply;

                knihovnaKomentareDao.Create(komentar);

                KnihovnaUserDao dao = new KnihovnaUserDao();
                KnihovnaUser user = dao.GetByLogin(User.Identity.Name);

                UserStats.NewComment(dao, user);

                NovinkyGenerator.PridatNovinku(komentar, user.Id);
            }

        return Redirect(this.Request.UrlReferrer.AbsolutePath);
        }

        [Authorize]
        public ActionResult Edit(int id, string text)
        {
            KnihovnaKomentareDao knihovnaKomentareDao = new KnihovnaKomentareDao();
            KnihovnaKomentare komentar = knihovnaKomentareDao.GetbyId(id);

            komentar.Content = text;
            knihovnaKomentareDao.Update(komentar);

            return Redirect(this.Request.UrlReferrer.AbsolutePath);
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            KnihovnaKomentareDao d = new KnihovnaKomentareDao();
            KnihovnaKomentare k = d.GetbyId(id);

            if (k.ReplyId != -1)
            {
                d.Delete(k);
            }
            else
            {
                // Musíme odstranit všechny reagující komentáře
                IList<KnihovnaKomentare> reagujiciKomentare = d.GetCommentSubcomments(id);

                foreach (KnihovnaKomentare sk in reagujiciKomentare)
                {
                    d.Delete(sk);
                }

                d.Delete(k);
            }

            return Redirect(this.Request.UrlReferrer.AbsolutePath);
        }
    }
}