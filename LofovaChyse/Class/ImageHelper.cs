using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using DataAccess.Dao;
using DataAccess.Models;
using System.Web.Mvc;

namespace LofovaChyse.Class
{
    public class ImageHelper
    {
        public static Image ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double) maxWidth / image.Width;
            var ratioY = (double) maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int) (image.Width * ratio);
            var newHeight = (int) (image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);
            Graphics.FromImage(newImage).DrawImage(image, 0, 0, newWidth, newHeight);

            return newImage;
        }

        public static string StripTagsRegex(string source)
        {
            return Regex.Replace(source, "<.*?>", string.Empty);
        }

        public static int GetCurrentUserId(string name)
        {
            KnihovnaUserDao dao = new KnihovnaUserDao();
            KnihovnaUser user = dao.GetByLogin(name);

            return user.Id;
        }

        public static double GetCurrentUserMoney(string name)
        {
            KnihovnaUserDao dao = new KnihovnaUserDao();
            KnihovnaUser user = dao.GetByLogin(name);

            return user.Money;
        }
    }
}