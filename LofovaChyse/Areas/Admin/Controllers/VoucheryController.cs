using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAccess.Dao;
using DataAccess.Models;
using LofovaChyse.Class;

namespace LofovaChyse.Areas.Admin.Controllers
{
    public class VoucheryController : Controller
    {
        public ActionResult Index()
        {
            KnihovnaVoucheryArchetyp v = new KnihovnaVoucheryArchetyp();
            
            return View(v);
        }

        public ActionResult Zapis()
        {
            KnihovnaVouchery v = new KnihovnaVouchery();

            return View(v);
        }

        public ActionResult Vypis()
        {
            KnihovnaVoucheryDao d = new KnihovnaVoucheryDao();
            List<KnihovnaVouchery> l = d.GetAll() as List<KnihovnaVouchery>;

            return View(l);
        }

        [HttpPost]
        [Authorize(Roles = "knihovnik")]
        [ValidateInput(false)]
        public ActionResult Add(KnihovnaVoucheryArchetyp book)
        {
            if (ModelState.IsValid)
            {
                KnihovnaVoucheryArchetyp v = new KnihovnaVoucheryArchetyp();
                v.Name = book.Name;
                v.Description = book.Description;
                v.Type = book.Type;
                v.Id = Books.Counter();

                KnihovnaVoucheryArchetypDao d = new KnihovnaVoucheryArchetypDao();
                d.Create(v);

                // Notifikace
                TempData["scs"] = "V pořádku";
            }
            else
            {
                return View("Index", book); // Vrátím vstupní data
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Authorize(Roles = "knihovnik")]
        [ValidateInput(false)]
        public ActionResult Pridat(KnihovnaVouchery book)
        {
            if (ModelState.IsValid)
            {
                KnihovnaVouchery v = new KnihovnaVouchery();
                v.Id = Books.Counter();
                v.Code = book.Code;
                v.Type = book.Type;
                v.Assigned = false;
                v.Used = false;
                v.UsedDate = DateTime.MinValue;
                v.UsedId = -1;


                KnihovnaVoucheryDao d = new KnihovnaVoucheryDao();
                d.Create(v);

                // Notifikace
                TempData["scs"] = "V pořádku";
            }
            else
            {
                return View("Zapis", book); // Vrátím vstupní data
            }

            return RedirectToAction("Index", "Home");
        }
    }
}