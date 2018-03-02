using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LofovaChyse.Controllers
{
    public class TinyMceController : Controller
    {
        // GET: TinyMce
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(string data)
        {
            var message = "Hello " + data;
            return Content(message);
        }

        [HttpPost]
        public ActionResult TinyMceUpload(HttpPostedFileBase file)
        {
            //Response.AppendHeader("Access-Control-Allow-Origin", "*");

            var location = SaveFile(Server.MapPath("~/Uploads/UsersContent/"), file);

            return Json(new {location}, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Saves the contents of an uploaded image file.
        /// </summary>
        /// <param name="targetFolder">Location where to save the image file.</param>
        /// <param name="file">The uploaded image file.</param>
        /// <exception cref="InvalidOperationException">Invalid MIME content type.</exception>
        /// <exception cref="InvalidOperationException">Invalid file extension.</exception>
        /// <exception cref="InvalidOperationException">File size limit exceeded.</exception>
        /// <returns>The relative path where the file is stored.</returns>
        private static string SaveFile(string targetFolder, HttpPostedFileBase file)
        {
            const int megabyte = 1024 * 1024;

            if (!file.ContentType.StartsWith("image/"))
            {
                throw new InvalidOperationException("Invalid MIME content type.");
            }

            var extension = Path.GetExtension(file.FileName.ToLowerInvariant());
            string[] extensions = {".gif", ".jpg", ".png", ".svg", ".webp"};
            if (!extensions.Contains(extension))
            {
                throw new InvalidOperationException("Invalid file extension.");
            }

            if (file.ContentLength > (8 * megabyte))
            {
                throw new InvalidOperationException("File size limit exceeded.");
            }

            var fileName = Guid.NewGuid() + extension;
            var path = Path.Combine(targetFolder, fileName);
            file.SaveAs(path);

            return Path.Combine("/Uploads/UsersContent", fileName).Replace('\\', '/');
        }
    }
}