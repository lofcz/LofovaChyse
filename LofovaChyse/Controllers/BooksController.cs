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

            if (User.Identity.IsAuthenticated)
            {
                foreach (Book b in books)
                {
                    if (CurrentUserRatedBook(b))
                    {
                        b.RateValue = -1;
                    }
                    else
                    {
                        b.RateValue = 1;
                    }
                }
            }

            return View(books); // Passnu třídu
        }

        public ActionResult Detail(int id, bool zobrazPopis = true)
        {
            BookDao bookDao = new BookDao();
            Book b = bookDao.GetbyId(id);

            ViewBag.Zobraz = zobrazPopis;

            KnihovnaKomentareDao knihovnaKomentareDao = new KnihovnaKomentareDao();
            IList<KnihovnaKomentare> knihovnaKomentare = knihovnaKomentareDao.GetAll();
            IList<KnihovnaKomentare> filtrovaneKomentare = new List<KnihovnaKomentare>();

            foreach (KnihovnaKomentare koment in knihovnaKomentare)
            {
                if (koment.TopicId == id)
                {
                    filtrovaneKomentare.Add(koment);
                }
            }

            ViewBag.Komentare = filtrovaneKomentare;

            return View(b);
        }

        public ActionResult Rate(int id, int value = 0)
        {
            BookLikesDao bookLikesDao = new BookLikesDao();
            IList<BookLikes> list = bookLikesDao.GetAll();

            BookDao bookDao = new BookDao();
            IList<Book> books = bookDao.GetAll();

            BookLikes finalBL = null;
            int UserId = new KnihovnaUserDao().GetByLogin(User.Identity.Name).Id;


            foreach (BookLikes bl in list)
            {
                if (bl.UserId == UserId && bl.BookId == id)
                {
                    finalBL = bl;
                    break;
                }
            }

            if (finalBL == null)
            {
                value = 1;

            }
            else
            {
                value = -1;
            }

            // Like
            if (value == 1)
            {
                if (finalBL == null)
                {
                    BookLikes like = new BookLikes();
                    like.Value = value;
                    like.UserId = new KnihovnaUserDao().GetByLogin(User.Identity.Name).Id;
                    like.BookId = id;

                    bookLikesDao.Create(like);
                }
            }
            else if (value == -1) // Unlike
            {
                foreach (BookLikes bl in list)
                {
                    if (bl.UserId == UserId && bl.BookId == id)
                    {
                        finalBL = bl;
                        break;
                    }
                }

                if (finalBL != null)
                {
                    bookLikesDao.Delete(finalBL);
                }

            }

            if (User.Identity.IsAuthenticated)
            {
                foreach (Book b in books)
                {
                    if (CurrentUserRatedBook(b))
                    {
                        b.RateValue = -1;
                    }
                    else
                    {
                        b.RateValue = 1;
                    }
                }
            }

            if (Request.IsAjaxRequest())
            {
               
                return PartialView("Index", books);
            }

           // return RedirectToAction("Index");
            return View("Index", books);
        }

        public string BookRating(int id)
        {
            int value = 0;
            BookLikesDao bookLikesDao = new BookLikesDao();
            IList<BookLikes> list = bookLikesDao.GetAll();

            foreach (BookLikes bl in list)
            {
                if (bl.BookId == id)
                {
                    value += bl.Value;
                }
            }

            return value.ToString();
        }

        public bool CurrentUserRatedBook(Book b)
        {
            int id = b.Id;

            BookLikesDao bookLikesDao = new BookLikesDao();
            IList<BookLikes> list = bookLikesDao.GetAll();

            int UserId = new KnihovnaUserDao().GetByLogin(User.Identity.Name).Id;

            foreach (BookLikes bl in list)
            {
                if (bl.UserId == UserId && bl.BookId == id)
                {
                    return true;
                }
            }

            return false;
        }

        [Authorize(Roles = "knihovnik")]
        public ActionResult Create()
        {
            BookCategoryDao bookCategoryDao = new BookCategoryDao();
            IList<BookCategory> categories = bookCategoryDao.GetAll();
            ViewBag.Categories = categories;

            return View();
        }

        [HttpPost] // post only
        [Authorize(Roles = "knihovnik")]
        public ActionResult Add(Book book, HttpPostedFileBase picture, int categoryId)
        {
            if (ModelState.IsValid)
            {
                Book b = new Book()
                {
                    Name = book.Name,
                    Author = book.Author,
                    PublishedYear = book.PublishedYear,
                    Id = Books.Counter(),
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

        [Authorize(Roles = "knihovnik")]
        public ActionResult Edit(int id)
        {
            BookDao bookDao = new BookDao();
            BookCategoryDao bookCategoryDao = new BookCategoryDao();

            Book b = bookDao.GetbyId(id);
            ViewBag.Categories = bookCategoryDao.GetAll();

            return View(b);
        }

        [Authorize(Roles = "knihovnik")]
        [HttpPost]
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

        [Authorize(Roles = "knihovnik")]
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