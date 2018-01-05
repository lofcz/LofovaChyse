using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAccess.Models;
using LofovaChyse.Class;

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
        public ActionResult Add(Book book, HttpPostedFileBase picture)
        {
            if (ModelState.IsValid)
            {
                Book b = new Book()
                {
                    Name = book.Name,
                    Author = book.Author,
                    PublishedYear = book.PublishedYear,
                    Id = Books.Counter,
                    Description = book.Description
                };
               

                if (picture != null)
                {
                    var z = picture.ContentLength;

                    if (picture.ContentType == "image/jpeg" || picture.ContentType == "image/png")
                    {
                        Image image = Image.FromStream(picture.InputStream);
                        image = ImageHelper.ScaleImage(image, 200, 200);


                        Bitmap btmBitmap = new Bitmap(image);
                        Guid guid = Guid.NewGuid();

                        string imageName = guid.ToString() + ".png";
                        btmBitmap.Save(Server.MapPath("~/Uploads/Book/") + imageName, ImageFormat.Png); // Je potřeba namapovat cestu!

                        btmBitmap.Dispose();
                        image.Dispose();

                        b.ImageName = imageName;
                    }
                }

                var bd = book.ImageName;
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