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

namespace LofovaChyse.Controllers
{
    public class BooksController : Controller
    {
        // GET: Books
        public ActionResult Index()
        {
            string pozdrav = "Lof lof";
            int cislo = 12;

            // Potřebuju uložit do kontaineru abych to dostal do view
            ViewBag.Pozdrav = pozdrav;
            ViewBag.Cislo = cislo;

            BookDao bookDao = new BookDao();
            IList<Book> books = bookDao.GetAll();

            return View(books); // Passnu třídu
        }

        public ActionResult Detail(int id, bool zobrazPopis)
        {
            BookDao bookDao = new BookDao();
            Book b = bookDao.GetbyId(id);

            ViewBag.Zobraz = zobrazPopis;

            return View(b);
        }

        public ActionResult Create()
        {
            BookCategoryDao bookCategoryDao = new BookCategoryDao();
            IList<BookCategory> categories = bookCategoryDao.GetAll();
            ViewBag.Categories = categories;

            return View();
        }

        [HttpPost] // post only
        public ActionResult Add(Book book, HttpPostedFileBase picture, int categoryId)
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
                        Image smalImage = ImageHelper.ScaleImage(image, 200, 200);


                        Bitmap btmBitmap = new Bitmap(smalImage);
                        Guid guid = Guid.NewGuid();

                        string imageName = guid.ToString() + ".jpg";
                        btmBitmap.Save(Server.MapPath("~/Uploads/Book/") + imageName, ImageFormat.Jpeg); // Je potřeba namapovat cestu!

                        btmBitmap.Dispose();
                        image.Dispose();

                        b.ImageName = imageName;
                    }
                }

                BookCategoryDao bookCategoryDao = new BookCategoryDao();
                BookCategory bookCategory = bookCategoryDao.GetbyId(categoryId);

                b.Category = bookCategory;

                BookDao bookDao = new BookDao();
                bookDao.Create(b);

                // Notifikace
                TempData["scs"] = "V pořádku";
            }
            else
            {
                return View("Create", book); // Vrátím vstupní data
            }

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            BookDao bookDao = new BookDao();
            BookCategoryDao bookCategoryDao = new BookCategoryDao();

            Book b = bookDao.GetbyId(id);
            ViewBag.Categories = bookCategoryDao.GetAll();

            return View(b);
        }

        public ActionResult Update(Book book, HttpPostedFileBase picture, int categoryId)
        {
            try
            {
                BookDao bookDao = new BookDao();
                BookCategoryDao bookCategoryDao = new BookCategoryDao();

                BookCategory bookCategory = bookCategoryDao.GetbyId(categoryId);

                book.Category = bookCategory;

                if (picture != null)
                {
                    Image image = Image.FromStream(picture.InputStream);
                    Image smalImage = ImageHelper.ScaleImage(image, 200, 200);

                    Bitmap btmBitmap = new Bitmap(smalImage);
                    Guid guid = Guid.NewGuid();

                    string imageName = guid.ToString() + ".jpg";
                    btmBitmap.Save(Server.MapPath("~/Uploads/Book/") + imageName, ImageFormat.Jpeg); // Je potřeba namapovat cestu!

                    btmBitmap.Dispose();
                    image.Dispose();

                    System.IO.File.Delete(Server.MapPath("~/Uploads/Book/") + book.ImageName);

                    book.ImageName = imageName;
                }

                bookDao.Update(book);

                TempData["scs"] = "Kniha " + book.Name + " byla upravena";
            }
            catch (Exception e)
            {
               
                throw;
            }

            return RedirectToAction("Index", "Books");
        }

        public ActionResult Delete(int id)
        {
            try
            {
                BookDao bookDao = new BookDao();
                Book book = bookDao.GetbyId(id);

                System.IO.File.Delete(Server.MapPath("~/Uploads/Book/") + book.ImageName);

                bookDao.Delete(book);
                TempData["scs"] = "Kniha " + book.Name + " byla smazána";
            }
            catch (Exception e)
            {
                throw;
            }

            return RedirectToAction("Index", "Books");
        }
    }
}