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

            List<Book> books = new List<Book>();
            books.Add(new Book() {Author = "Satan", Id = 1, Name = "Bible of the beast", PublishedYear = 666});
            books.Add(new Book() { Author = "Satan2", Id = 2, Name = "Bible of the beast2", PublishedYear = 666 });
            books.Add(new Book() { Author = "Satan3", Id = 3, Name = "Bible of the beast3", PublishedYear = 666 });
            books.Add(new Book() { Author = "Satan4", Id = 4, Name = "Bible of the beast4", PublishedYear = 666 });


            // Potřebuju ulořit do kontaineru abych to dostal do view
            ViewBag.Pozdrav = pozdrav;
            ViewBag.Cislo = cislo;

            return View(books); // Pasnu třídu
        }
    }
}