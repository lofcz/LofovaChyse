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
    public class ProfileController : Controller
    {
        // GET: Profile
        [Authorize]
        public ActionResult Index(int id)
        {
            KnihovnaUserDao knihovnaUserDao = new KnihovnaUserDao();
            KnihovnaUser knihovnaUser = knihovnaUserDao.GetbyId(id);

            bool v = false;

            if (knihovnaUser.Name == knihovnaUserDao.GetByLogin(User.Identity.Name).Name)
            {
                v = true;
            }
            else
            {
                v = false;
            }

            ViewBag.Owner = v;
            return View(knihovnaUser);
        }

        [ValidateInput(false)]
        public ActionResult EditProfile(string welcomeText)
        {
            KnihovnaUserDao knihovnaUserDao = new KnihovnaUserDao();
            KnihovnaUser knihovnaUser = knihovnaUserDao.GetByLogin(User.Identity.Name);
           
            knihovnaUser.WelcomeText = welcomeText;

            knihovnaUserDao.Update(knihovnaUser);

            return RedirectToAction("Index", new {id = knihovnaUser.Id});
        }

        [Authorize]
        public ActionResult ViewWall(int id)
        {
            KnihovnaUserDao knihovnaUserDao = new KnihovnaUserDao();
            KnihovnaUser knihovnaUser = knihovnaUserDao.GetbyId(id);

            bool v = false;

            if (knihovnaUser.Name == knihovnaUserDao.GetByLogin(User.Identity.Name).Name)
            {
                v = true;
            }
            else
            {
                v = false;
            }

            ViewBag.Owner = v;
            ViewBag.OwnerName = knihovnaUser.Name;
            ViewBag.OwnerObject = knihovnaUser;
            ViewBag.OwnerID = knihovnaUser.Id;

            KnihovnaZedDao zedDao = new KnihovnaZedDao();

            IList<KnihovnaZed> knihovnaZed = zedDao.GetUserWall(id);

            return View(knihovnaZed);
        }

        public bool GetDisplayText(int id)
        {
            KnihovnaUserDao k = new KnihovnaUserDao();
            KnihovnaUser u = k.GetbyId(id);

            return u.DisplayTextNo;
        }

        [Authorize]
        public ActionResult AddToWall(KnihovnaZed zed, int userId)
        {
            if (ModelState.IsValid)
            {
                KnihovnaZedDao dao = new KnihovnaZedDao();

                KnihovnaZed novaZed = new KnihovnaZed();
                novaZed.Id = zed.Id;
                novaZed.PostDate = zed.PostDate;
                novaZed.PostOwner = new KnihovnaUserDao().GetByLogin(User.Identity.Name);
                novaZed.PostText = zed.PostText;
                novaZed.Version = 1;

                dao.Create(novaZed);
            }

            KnihovnaZedDao zedDao = new KnihovnaZedDao();

            IList<KnihovnaZed> knihovnaZed = zedDao.GetUserWall(userId);

            return Redirect(Request.UrlReferrer.ToString());
        }

        [Authorize]
        public ActionResult ViewAchievements(string user)
        {
            KnihovnaUser u = new KnihovnaUserDao().GetByLogin(user);
            return PartialView(u);
        }

        [Authorize]
        public ActionResult NastavitInformace(string user)
        {
            KnihovnaUser u = new KnihovnaUserDao().GetByLogin(user);
            return PartialView(u);
        }

        public JsonResult SetInformationVisibility(string id, string state, string checkId)
        {
            KnihovnaUserDao d = new KnihovnaUserDao();
            KnihovnaUser u = d.GetbyId(int.Parse(id));

            if (checkId == "myCheckbox1") { u.DisplayJob = Boolean.Parse(state);}
            if (checkId == "myCheckbox2") { u.DisplayEducation = Boolean.Parse(state); }
            if (checkId == "myCheckbox3") { u.DisplayHobbies = Boolean.Parse(state); }
            if (checkId == "myCheckbox4") { u.DisplayBooks = Boolean.Parse(state); }
            if (checkId == "myCheckbox5") { u.DisplayMusic = Boolean.Parse(state); }
            if (checkId == "myCheckbox6") { u.DisplayMovies = Boolean.Parse(state); }
            if (checkId == "myCheckbox7") { u.DisplayName = Boolean.Parse(state); }
            if (checkId == "myCheckbox8") { u.DisplayAge = Boolean.Parse(state); }
            if (checkId == "myCheckbox9") { u.DisplayProfileViews = Boolean.Parse(state); }
            if (checkId == "myCheckbox10") { u.DisplayTextNo = Boolean.Parse(state); }
            if (checkId == "myCheckbox11") { u.DisplayStatisticsNo = Boolean.Parse(state); }
            if (checkId == "myCheckbox12") { u.DisplayMedalsNo = Boolean.Parse(state); }
            if (checkId == "myCheckbox13") { u.DisplayHelpNo = Boolean.Parse(state); }
            if (checkId == "myCheckbox14") { u.DisplayPrivateProfile = Boolean.Parse(state); }

            d.Update(u);

            return Json(new { });
        }


        [Authorize]
        public JsonResult ProfilUpdateVzdelani(string value)
        {
            KnihovnaUserDao d = new KnihovnaUserDao();
            KnihovnaUser u = d.GetByLogin(User.Identity.Name);

            u.Education = value;
            d.Update(u);

            return Json(new { });
        }

        [Authorize]
        public JsonResult ProfilUpdateKonicky(string value)
        {
            KnihovnaUserDao d = new KnihovnaUserDao();
            KnihovnaUser u = d.GetByLogin(User.Identity.Name);

            u.Hobbies = value.Replace(",", ", ");
            d.Update(u);

            return Json(new { });
        }

        [Authorize]
        public JsonResult ProfilUpdateKnihy(string value)
        {
            KnihovnaUserDao d = new KnihovnaUserDao();
            KnihovnaUser u = d.GetByLogin(User.Identity.Name);

            u.Literature = value;
            d.Update(u);

            return Json(new { });
        }

        [Authorize]
        public JsonResult ProfilUpdateFilmy(string value)
        {
            KnihovnaUserDao d = new KnihovnaUserDao();
            KnihovnaUser u = d.GetByLogin(User.Identity.Name);

            u.Movies = value;
            d.Update(u);

            return Json(new { });
        }

        [Authorize]
        public JsonResult ProfilUpdateHudba(string value)
        {
            KnihovnaUserDao d = new KnihovnaUserDao();
            KnihovnaUser u = d.GetByLogin(User.Identity.Name);

            u.Music = value;
            d.Update(u);

            return Json(new { });
        }

        [Authorize]
        public JsonResult ProfilUpdateText(string value)
        {
            KnihovnaUserDao d = new KnihovnaUserDao();
            KnihovnaUser u = d.GetByLogin(User.Identity.Name);

            u.WelcomeText = value;
            d.Update(u);

            return Json(new { });
        }

        [Authorize]
        public ActionResult EditImage(HttpPostedFileBase picture)
        {
            KnihovnaUserDao d = new KnihovnaUserDao();
            KnihovnaUser u = d.GetByLogin(User.Identity.Name);

            if (picture != null)
            {
                Image image = Image.FromStream(picture.InputStream);
                Image smalImage = ImageHelper.ResizeImageHighQuality(image, 32, 32);

                Bitmap btmBitmap = new Bitmap(smalImage);
                Guid guid = Guid.NewGuid();

                string imageName = guid.ToString() + ".png";
                btmBitmap.Save(Server.MapPath("~/Uploads/KnihovnaUzivatele/") + imageName, ImageFormat.Png);

                btmBitmap.Dispose();

                // HiRes save
                Image bigImage = ImageHelper.ResizeImageHighQuality(image, 200, 200);
                Bitmap btmBitmap2 = new Bitmap(bigImage);

                string imageName2 = guid.ToString() + ".png";
                btmBitmap2.Save(Server.MapPath("~/Uploads/KnihovnaUzivateleBig/") + imageName2, ImageFormat.Png);

                btmBitmap2.Dispose();
                bigImage.Dispose();

                image.Dispose();


                u.ImageName = imageName;
            }


            d.Update(u);

            return Redirect(Request.UrlReferrer.ToString());
        }
        

    }
}