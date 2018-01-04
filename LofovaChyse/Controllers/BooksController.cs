using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAccess.Models;

namespace LofovaChyse.Controllers
{
    public class BooksController : Controller
    {
        // GET: Books
        public ActionResult Index()
        {
            string pozdrav = "Lof lof";
            int cislo = 12;

            // Potřebuju ulořit do kontaineru abych to dostal do view
            ViewBag.Pozdrav = pozdrav;
            ViewBag.Cislo = cislo;

            return View(Books.GetFakeList); // Passnu třídu
        }

        public ActionResult Detail(int? id, bool zobrazPopis)
        {
            Book b = (from Book book in Books.GetFakeList where book.Id == id select book).FirstOrDefault();
            ViewBag.Zobraz = zobrazPopis;

            return View(b);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost] // post only
        public ActionResult Add(Book book)
        {
            if (ModelState.IsValid)
            {
                Book b = new Book()
                {
                    Name = book.Name,
                    Author = book.Author,
                    PublishedYear = book.PublishedYear,
                    Id = Books.Counter
                };
                Books.GetFakeList.Add(b);
            }
            else
            {
                return View("Create", book); // Vrátím vstupní data
            }

            return RedirectToAction("Index");
        }
    }
}