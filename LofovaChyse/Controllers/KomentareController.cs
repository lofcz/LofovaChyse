using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public ActionResult Add(string description, int topicId, int reply, string type = "", string typeData = "", string sumText = "")
        {
            if (type != "create" && type != "")
            {
                if (typeData == "komentEdit")
                {
                    Edit(int.Parse(type), description);
                }
                else
                {
                    EditPrispevek(topicId, description, sumText);
                }
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
        public ActionResult EditPrispevek(int id, string text, string sumText)
        {
            BookDao d = new BookDao();
            Book b = d.GetbyId(id);
            b.Description = text;
            b.Version += 0.1;
            d.Update(b);

            // Zaneseme verzování
            KnihovnaUser usr = new KnihovnaUserDao().GetByLogin(User.Identity.Name);

            BookVersion ver = new BookVersion();
            ver.Id = Books.Counter();
            ver.SumText = sumText;
            ver.Version = b.Version;
            ver.ChangedBy = usr.Id;
            ver.Date = DateTime.Now;
            ver.IsApproved = false;
            ver.IsSuggestion = false;
            ver.Text = b.Description;
            ver.PostId = b.Id;

            BookVersionDao vd = new BookVersionDao();
            vd.Create(ver);

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