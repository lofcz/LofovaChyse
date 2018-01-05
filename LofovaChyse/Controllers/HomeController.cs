using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}