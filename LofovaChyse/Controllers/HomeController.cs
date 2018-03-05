using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Web;
using System.Web.Mvc;
using DataAccess.Dao;
using DataAccess.Models;

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
    }
}