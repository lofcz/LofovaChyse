using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using DataAccess.Dao;
using DataAccess.Models;
using LofovaChyse.Class;

namespace LofovaChyse.Controllers
{
    public class BooksController : Controller
    {
        // GET: Books
        public ActionResult Index(int? page, int? cat)
        {
            int itemsOnPage = 5;
            int pg = page.HasValue ? page.Value : 1;
            int totalBooks;

            if (!cat.HasValue)
            {
                cat = null;
            }

            string pozdrav = "Lof lof";
            int cislo = 12;

            // Potřebuju uložit do kontaineru abych to dostal do view
            ViewBag.Pozdrav = pozdrav;
            ViewBag.Cislo = cislo;
            ViewBag.Cat = cat;

            BookDao bookDao = new BookDao();

            IList<Book> bPaged = bookDao.GetBooksPaged(itemsOnPage, pg, out totalBooks, cat);

            //IList<Book> booksPaged = bookDao.GetBooksInSection(cat);//bookDao.GetBooksPaged(itemsOnPage, pg, out totalBooks);
            List<Book> u = bPaged as List<Book>;

            bPaged = bPaged.OrderBy(x => x.Id).ToList();

            if (User.Identity.IsAuthenticated)
            {
                foreach (Book b in bPaged)
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
            
            KnihovnaUser user = new KnihovnaUserDao().GetByLogin(User.Identity.Name);

            ViewBag.Pages = (int)Math.Ceiling((double)totalBooks / (double)itemsOnPage);
            ViewBag.CurrentPage = pg;
            ViewBag.PerPage = itemsOnPage;
            ViewBag.total = totalBooks;

            if (Request.IsAjaxRequest())
            {
                return PartialView(u);
            }

            return View(u); // Passnu třídu
        }

        public ActionResult SearchModal()
        {
            return PartialView();
        }

        public bool CurrentUserRatedComent(KnihovnaKomentare komentar)
        {
            if (User.Identity.IsAuthenticated)
            {
                KnihovnaKomentareLikesDao knihovnaKomentareDao = new KnihovnaKomentareLikesDao();
                IList<KnihovnaKomentareLikes> list = knihovnaKomentareDao.GetAll();

                KnihovnaKomentareLikes finalLike = null;
                int userId = new KnihovnaUserDao().GetByLogin(User.Identity.Name).Id;

                foreach (KnihovnaKomentareLikes iterovanyLike in list)
                {
                    if (iterovanyLike.UserId == userId && iterovanyLike.ComentId == komentar.Id)
                    {
                        finalLike = iterovanyLike;
                        break;
                    }
                }

                if (finalLike == null)
                {
                    return false;
                }
            }

            return true;
        }
        public int CurrentUserRatedComentType(KnihovnaKomentare komentar)
        {
            int r = -1;

            if (User.Identity.IsAuthenticated)
            {
                KnihovnaKomentareLikesDao knihovnaKomentareDao = new KnihovnaKomentareLikesDao();
                IList<KnihovnaKomentareLikes> list = knihovnaKomentareDao.GetAll();

                KnihovnaKomentareLikes finalLike = null;
                int userId = new KnihovnaUserDao().GetByLogin(User.Identity.Name).Id;

                foreach (KnihovnaKomentareLikes iterovanyLike in list)
                {
                    if (iterovanyLike.UserId == userId && iterovanyLike.ComentId == komentar.Id)
                    {
                        finalLike = iterovanyLike;
                        r = finalLike.Value;
                        break;
                    }
                }
            }
          
            return r;
        }
        public ActionResult Buy(int id, string userName)
        {
            BookDao bookDao = new BookDao();
            Book b = bookDao.GetbyId(id);

            ViewBag.Zobraz = true;
            ViewBag.Cost = b.UnlockPrice;
            ViewBag.BuyId = id;

            KnihovnaUserDao dao = new KnihovnaUserDao();
            KnihovnaUser u = dao.GetByLogin(userName);

            return PartialView(u);
        }
        public ActionResult CompleteBuy(int id, string userName, int buyCost)
        {
            KnihovnaUserDao d = new KnihovnaUserDao();
            BookPaymentDao dao = new BookPaymentDao();
            BookPayment payment = new BookPayment();

            payment.Id = Books.Counter();
            payment.DateUnlocked = DateTime.Now;
            payment.IsPreview = false;
            payment.PostId = id;
            payment.UserId = d.GetByLogin(userName).Id;

            KnihovnaUser user = d.GetByLogin(userName);
            user.Money -= buyCost;

            dao.Create(payment);
            d.Update(user);

            return Redirect(Request.UrlReferrer.ToString());
        }
        public ActionResult Detail(int id, bool zobrazPopis = true)
        {
            BookDao bookDao = new BookDao();
            BookLikesDao palce = new BookLikesDao();
            KnihovnaUserDao uzivatele = new KnihovnaUserDao();

            KnihovnaUser myUser = uzivatele.GetByLogin(User.Identity.Name);

            Book b = bookDao.GetbyId(id);

            b.PocetReakci[0] = palce.GetBookLikes(b.Id, 0) ?? 0;
            b.PocetReakci[1] = palce.GetBookLikes(b.Id, 1) ?? 0;
            b.PocetReakci[2] = palce.GetBookLikes(b.Id, 2) ?? 0;
            b.PocetReakci[3] = palce.GetBookLikes(b.Id, 3) ?? 0;

            if (User.Identity.IsAuthenticated)
            {
                b.CurrentUserReakce = General.GetBookUserRating(b.Id, myUser.Id);
            }
            else
            {
                b.CurrentUserReakce = -2;
            }




            ViewBag.Zobraz = zobrazPopis;

            KnihovnaKomentareDao knihovnaKomentareDao = new KnihovnaKomentareDao();
            IList<KnihovnaKomentare> knihovnaKomentare = knihovnaKomentareDao.GetAll();
            IList<KnihovnaKomentare> filtrovaneKomentare = new List<KnihovnaKomentare>();

            foreach (KnihovnaKomentare koment in knihovnaKomentare)
            {
                if (koment.TopicId == id)
                {
                    koment.AlreadyRated = CurrentUserRatedComent(koment);

                    if (koment.AlreadyRated)
                    {
                        koment.RatedType = CurrentUserRatedComentType(koment);
                    }
                    else
                    {
                        koment.RatedType = -1;
                    }

                    // Reakce:
                    for (int i = 0; i < 4; i++)
                    {
                        int? pocetReakci = new KnihovnaKomentareLikesDao().GetComentLikes(koment.Id, i);
                        if (pocetReakci.HasValue)
                        {
                            koment.PocetReakci[i] = (int)pocetReakci;
                        }
                        else
                        {
                            koment.PocetReakci[i] = 0;
                        }
                    }

                    filtrovaneKomentare.Add(koment);
                }
            }

            // Seřadíme reply comenty
            IList<KnihovnaKomentare> filtrovaneKomentareBackup = filtrovaneKomentare.ToList();
            IList<KnihovnaKomentare> posledniKomenty = new List<KnihovnaKomentare>();

            foreach (KnihovnaKomentare koment in filtrovaneKomentareBackup)
            {
                if (koment.ReplyId <= 0)
                {
                    posledniKomenty.Add(koment);
                    // Přidáme všechny subkomenty
                    foreach (KnihovnaKomentare k in filtrovaneKomentareBackup)
                    {
                        if (k.ReplyId == koment.Id)
                        {
                            posledniKomenty.Add(k);
                        }
                    }
                }
            }

            ViewBag.Komentare = posledniKomenty;

            return View(b);
        }
        public ActionResult RateKomentar(int bookId, int id, int moznost, int komentId = -1)
        {
            KnihovnaUserDao dd = new KnihovnaUserDao();
            Book book = new BookDao().GetbyId(bookId);
            int value = 0;

            KnihovnaKomentareLikesDao knihovnaKomentareLikesDao = new KnihovnaKomentareLikesDao();
            IList<KnihovnaKomentareLikes> list = knihovnaKomentareLikesDao.GetAll();
            KnihovnaKomentare k = new KnihovnaKomentareDao().GetbyId(id);

            KnihovnaKomentareLikes finalLike = null;
            int userId = dd.GetByLogin(User.Identity.Name).Id;

            foreach (KnihovnaKomentareLikes iterovanyLike in list)
            {
                if (iterovanyLike.UserId == userId && iterovanyLike.ComentId == id)
                {
                    finalLike = iterovanyLike;
                    break;
                }
            }

            if (finalLike == null && userId != k.OwnerId.Id) {value = 1;}
            else {value = -1;}

            // Hodnotit
            if (value == 1)
            {
                finalLike = new KnihovnaKomentareLikes();
                finalLike.Id = Books.Counter();
                finalLike.ComentId = id;
                finalLike.UserId = dd.GetByLogin(User.Identity.Name).Id;
                finalLike.Value = moznost;

                KnihovnaUser userFrom = dd.GetByLogin(User.Identity.Name);
                KnihovnaUser userTo = dd.GetbyId(k.OwnerId.Id);
                HNotifikace.SendRateNotification(userTo, moznost, userFrom, book);

                UserStats.NewRating(dd, userFrom);

                knihovnaKomentareLikesDao.Create(finalLike);
            }

            IList<KnihovnaKomentare> kolekceKomentu = new KnihovnaKomentareDao().GetAll();
            IList<KnihovnaKomentare> finalniKomenty = new List<KnihovnaKomentare>();

            foreach (KnihovnaKomentare koment in kolekceKomentu)
            {
                koment.AlreadyRated = CurrentUserRatedComent(koment);

                if (koment.TopicId == book.Id)
                {
                    koment.AlreadyRated = CurrentUserRatedComent(koment);

                    if (koment.AlreadyRated)
                    {
                        koment.RatedType = CurrentUserRatedComentType(koment);
                    }
                    else
                    {
                        koment.RatedType = -1;
                    }

                    // Reakce:
                    for (int i = 0; i < 4; i++)
                    {
                        int? pocetReakci = new KnihovnaKomentareLikesDao().GetComentLikes(koment.Id, i);
                        if (pocetReakci.HasValue)
                        {
                            koment.PocetReakci[i] = (int)pocetReakci;
                        }
                        else
                        {
                            koment.PocetReakci[i] = 0;
                        }
                    }

                    finalniKomenty.Add(koment);
                }
            }

            ViewBag.Komentare = finalniKomenty;
            ViewBag.Zobraz = true;

            if (Request.IsAjaxRequest())
            {
                for (int i = 0; i < 4; i++)
                {
                    int? pocetReakci = new KnihovnaKomentareLikesDao().GetComentLikes(k.Id, i);
                    if (pocetReakci.HasValue)
                    {
                        k.PocetReakci[i] = (int)pocetReakci;
                    }
                    else
                    {
                        k.PocetReakci[i] = 0;
                    }
                }

                ViewBag.topic = bookId;
                k.RatedType = CurrentUserRatedComentType(k);
                return PartialView("Rating", k);
            }

            return View("Detail", book);
        }
        public ActionResult Rate(int id, int moznost)
        {
            int value = 0;

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
                    like.Value = moznost;
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

            List<Book> fb = books as List<Book>;

            if (Request.IsAjaxRequest())
            {
               
                return PartialView("Index", fb);
            }

           // return RedirectToAction("Index");
            return View("Index", fb);
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
        public ActionResult Create(int categoryId = 0)
        {
            BookCategoryDao bookCategoryDao = new BookCategoryDao();
            IList<BookCategory> categories = bookCategoryDao.GetAll();
            ViewBag.Categories = categories;

            ViewBag.kategorie = categoryId;

            return View();
        }

        [HttpPost] // post only
        [Authorize(Roles = "knihovnik")]
        public ActionResult Add(Book book, HttpPostedFileBase picture, int categoryId)
        {
            //if (ModelState.IsValid)
            {
                KnihovnaUserDao d = new KnihovnaUserDao();
                KnihovnaUser u = d.GetByLogin(User.Identity.Name);
                KnihovnaKategorieDao k = new KnihovnaKategorieDao();

                Book b = new Book()
                {
                    Name = book.Name,
                    Author = book.Author,
                    PublishedYear = book.PublishedYear,
                    Id = Books.Counter(),
                    Description = book.Description,
                    OwnerId =  u,
                    Kategorie = k.GetbyId(2),
                    LastEditDateTime = DateTime.Now,
                    Version = 1,
                    IsPayed = false,
                    UnlockPrice = 0,
                    MinimalLevel = 0
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
                BookCategory bookCategory = bookCategoryDao.GetbyId(5);

                b.Category = bookCategory;
                b.Author = "nějaká děvka";
                b.SectionId = categoryId;

                BookDao bookDao = new BookDao();
                int identifier = (int)bookDao.Create(b);

                BookVersionDao bv = new BookVersionDao();
                BookVersion v = new BookVersion();

                v.Text = b.Description;
                v.ChangedBy = u.Id;
                v.Date = DateTime.Now;
                v.Id = Books.Counter();
                v.IsApproved = true;
                v.IsSuggestion = false;
                v.PostId = identifier;
                v.SumText = "Původní verze";
                v.Version = 1;

                bv.Create(v);


                LevelUp.NewPost(u, d);
                NovinkyGenerator.PridatNovinku(b, u.Id);

                // Notifikace
                TempData["scs"] = "V pořádku";
            }
           // else
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
        [HttpPost, ValidateInput(false)]
        public ActionResult Update(Book book, HttpPostedFileBase picture, int categoryId, string sumText, string userName)
        {
            try
            {
                BookDao bookDao = new BookDao();
                BookCategoryDao bookCategoryDao = new BookCategoryDao();

                BookCategory bookCategory = bookCategoryDao.GetbyId(categoryId);

                book.Category = bookCategory;
                book.Version = book.Version + 0.1;
                book.LastEditDateTime = DateTime.Now;

                KnihovnaUser usr = new KnihovnaUserDao().GetByLogin(userName);

                // Zaneseme verzování
                BookVersion ver = new BookVersion();
                ver.Id = Books.Counter();
                ver.SumText = sumText;
                ver.Version = book.Version;
                ver.ChangedBy = usr.Id;
                ver.Date = DateTime.Now;
                ver.IsApproved = false;
                ver.IsSuggestion = false;
                ver.Text = book.Description;
                ver.PostId = book.Id;

                BookVersionDao vd = new BookVersionDao();
                vd.Create(ver);
                

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

        public ActionResult RateDenied()
        {
            return PartialView("RateDenied");
        }

        public ActionResult BookHistory(int id)
        {
            BookVersionDao d = new BookVersionDao();
            List<BookVersion> seznamEditu = d.GetBookVersions(id) as List<BookVersion>;

            return PartialView(seznamEditu);
        }

        public ActionResult BookHistoryShow(int id)
        {
            BookVersion v = new BookVersionDao().GetbyId(id);
            return PartialView(v);
        }

        public JsonResult BookHistoryShowJson(int id)
        {
            BookVersion v = new BookVersionDao().GetbyId(id);
            return Json(new { text = v.Text });
        }

        public ActionResult BookHistorySet(int id)
        {
            BookVersion v = new BookVersionDao().GetbyId(id);
            BookDao bd = new BookDao();
            Book b = bd.GetbyId(v.PostId);
            b.Version = v.Version;
            b.Description = v.Text;
            bd.Update(b);


            BookVersionDao d = new BookVersionDao();
            List<BookVersion> seznamEditu = d.GetBookVersions(v.PostId) as List<BookVersion>;

            return PartialView("BookHistory", seznamEditu);
        }
    }
}