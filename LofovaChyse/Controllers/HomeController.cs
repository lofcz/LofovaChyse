using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Web;
using System.Web.Mvc;
using DataAccess.Dao;
using DataAccess.Models;
using LofovaChyse.Class;

namespace LofovaChyse.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            // Dao test
            /*
            BookCategoryDao bcDao = new BookCategoryDao();
            IList<BookCategory> categories = bcDao.GetAll();

            BookCategory bookCategory = new BookCategory();
            bookCategory.Name = "Učebnice";
            bookCategory.Description = "Literatura o hovně";

            bcDao.Create(bookCategory);
            */

            return View();
        }

        public ActionResult Vypis()
        {
            return View();
        }

        public ActionResult AjaxRequest()
        {
            return View();
        }

        public ActionResult NastavNaladu(int checkedId = -1)
        {
            KnihovnaUserDao d = new KnihovnaUserDao();
            KnihovnaUser u = d.GetByLogin(User.Identity.Name);

            ViewBag.checkedId = u.NeedJob;
            return PartialView();
        }

        [HttpPost]
        [Authorize]
        public ActionResult NastavNaladuSQL(FormCollection frm, int checkedId = -1)
        {
            string text = frm["radio1"].ToString();

            if (text == "inp1")
            {
                checkedId = 1;
            }
            if (text == "inp2")
            {
                checkedId = 2;
            }
            if (text == "inp3")
            {
                checkedId = 3;
            }
            if (text == "inp4")
            {
                checkedId = 4;
            }

            // zapíšeme do db
            if (checkedId > 0)
            {
                KnihovnaUserDao d = new KnihovnaUserDao();
                KnihovnaUser u = d.GetByLogin(User.Identity.Name);

                u.NeedJob = checkedId;
                d.Update(u);
            }



            ViewBag.checkedId = checkedId;
            return PartialView();
        }

        public ActionResult Test(int id)
        {
            KnihovnaOceneniDao d = new KnihovnaOceneniDao();
            KnihovnaOceneni o = d.GetbyId(id);

            

            ViewBag.Id = id;
            ViewBag.Text = o.Text;

            return PartialView();
        }

        [ValidateInput(false)]
        [Authorize]
        public ActionResult Eval(string popis)
        {
            string header = @"List<int> inputList = new List<int>(); ";
            Random r = new Random();

            List<int> testData = new List<int>();          

            for (var i = 0; i < 10; i++)
            {
                testData.Add(r.Next(1, 100));
                header = header + "inputList.Add(" + testData[i] + "); ";
            }

            List<int> backup = testData.ToList();
            List<int> result = Z.Expressions.Eval.Execute<List<int>>(header + "\r\n" + popis);
           
            ViewBag.Result = result;
            ViewBag.RawData = backup.ToList();

            testData.Sort();
            ViewBag.Desired = testData;

            bool equal = ListEqual(result, testData);

            ViewBag.Ok = equal;

            return PartialView();
        }

        static bool ListEqual(List<int> list1, List<int> list2)
        {
            for (int i = 0; i < list1.Count; i++)
            {
                if (list1[i] != list2[i])
                {
                    return false;
                }
            }
            return true;
        }

        public ActionResult Home()
        {
            List<KnihovnaNovinky> list = new KnihovnaNovinkyDao().GetAll() as List<KnihovnaNovinky>;
            list.Sort((ps1, ps2) => DateTime.Compare(ps1.Date, ps2.Date));
            list.Reverse();

            return View(list);
        }

        public ActionResult UzivatelNahled()
        {
            return PartialView();
        }

        // From, to
        public JsonResult CallGeneral(string user1, string user2)
        {
            KnihovnaPrateleDao pd = new KnihovnaPrateleDao();
            KnihovnaUserDao d = new KnihovnaUserDao();

            // Najdeme oba účastníky
            KnihovnaUser from = d.GetByLogin(user1);
            KnihovnaUser to = d.GetByLogin(user2);

            // Sestavíme žádost
            KnihovnaPratele zadost = new KnihovnaPratele();
            zadost.Id = Books.Counter();
            zadost.Accepted = false;
            zadost.DateAccepted = DateTime.MinValue;
            zadost.DateDeclined = DateTime.MinValue;
            zadost.DateSent = DateTime.Now;
            zadost.Declined = false;
            zadost.Type = 0;
            zadost.UserFrom = from.Id;
            zadost.UserTo = to.Id;

            // Pošleme žádost
            pd.Create(zadost);

            // Žádaný obdrží notifikaci
            KnihovnaNotifikace n = new KnihovnaNotifikace();
            n.Id = Books.Counter();
            n.DateSent = DateTime.Now;
            n.Description = "Uživatel " + from.Name + " si tě chce přidat do přátel";
            n.Displayed = false;
            n.ForceType = 0;
            n.RewardType = -666;
            n.Text = "Uživatel " + from.Name + " si tě chce přidat do přátel";
            n.UserFrom = -1;
            n.UserTo = to.Id;
            n.ForceType = zadost.Id; // použijeme jako data-storage [FUJ, HACK]

            // Pošleme notifikaci
            KnihovnaNotifikaceDao dd = new KnihovnaNotifikaceDao();
            dd.Create(n);

            return Json(new {});
        }

        public JsonResult AcceptFriend(int id = 0, int notifikaceId = 0)
        {
            General.ConfirmFriendShip(id);

            KnihovnaUserDao d = new KnihovnaUserDao();
            KnihovnaUser u = d.GetByLogin(User.Identity.Name);

            KnihovnaNotifikaceDao dao = new KnihovnaNotifikaceDao();
            KnihovnaNotifikace n = dao.GetbyId(notifikaceId); 

            d.Update(u);
            dao.Delete(n);

            return Json(new { });
        }

        public JsonResult Dissmis(int notifikaceId = 0)
        {
            KnihovnaUserDao d = new KnihovnaUserDao();
            KnihovnaUser u = d.GetByLogin(User.Identity.Name);

            KnihovnaNotifikaceDao dao = new KnihovnaNotifikaceDao();
            KnihovnaNotifikace n = dao.GetbyId(notifikaceId);

            d.Update(u);
            dao.Delete(n);

            return Json(new { });
        }
    }
}