using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using DataAccess.Dao;
using DataAccess.Models;
using LofovaChyse.Class;

namespace LofovaChyse.Controllers
{
    public class ChatController : Controller
    {
        // GET: Chat
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult RestoreChat(string name)
        {
            name = User.Identity.Name;
            string s = "";

            KnihovnaUserDao ud = new KnihovnaUserDao();
            ChatZpravyDao d = new ChatZpravyDao();
            List<ChatZpravy> l = d.GetAll() as List<ChatZpravy>;
            KnihovnaUser cu = ud.GetByLogin(name);

            foreach (ChatZpravy z in l)
            {
                KnihovnaUser u = ud.GetbyId(z.UserFrom);

                // Zpráva od teve
                if (u.Id == cu.Id)
                {
                    s += "<span class='float-right chatMsgYour'>" + z.Text + "</span><br/>";
                }
                else
                {
                    s += "<img src='" + ("/Uploads/KnihovnaUzivatele/" + General.GetMiniaturePicture(u.Login)) + "' /> <span class='chatMsgHis'>" + z.Text + "</span><br/>";
                }

            }

            return Json(s);
        }
    }
}