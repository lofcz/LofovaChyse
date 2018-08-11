using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using DataAccess.Dao;
using DataAccess.Models;

namespace LofovaChyse.Class
{
    public class NovinkyGenerator
    {
        public enum typeToInt
        {
            Undefined,
            Book,
            User,
            Comment
        }

        public static void PridatNovinku<T>(T zdroj, int userId, int priority = 0, bool sticky = false)
        {
            typeToInt type = typeToInt.Undefined;
            int typeSub = 0;
            int refId = 0;
            int existingId = -1;
            string text = "";
            string postName = "";
            string action = "";
            string controller = "";


            KnihovnaNovinkyDao dao = new KnihovnaNovinkyDao();
            KnihovnaUser user = new KnihovnaUserDao().GetbyId(userId);

            if (zdroj is Book)
            {
                type = typeToInt.Book;
                refId = (zdroj as Book).Id;
                postName = (zdroj as Book).Name;
                action = "Detail";
                controller = "Books";

            }

            if (zdroj is KnihovnaUser)
            {
                type = typeToInt.User;
                refId = (zdroj as KnihovnaUser).Id;
                postName = (zdroj as KnihovnaUser).Name;
                action = "Index";
                controller = "Profile";
            }

            if (zdroj is KnihovnaKomentare)
            {
                type = typeToInt.Comment;
                refId = new BookDao().GetbyId((zdroj as KnihovnaKomentare).TopicId).Id;
                postName = new BookDao().GetbyId((zdroj as KnihovnaKomentare).TopicId).Name;
                action = "Detail";
                controller = "Books";
            }

            // 1) Check if same shit exists
            IList<KnihovnaNovinky> checkList = dao.GetNovinkyWhereType((int) type);

            foreach (KnihovnaNovinky n in checkList)
            {
                if (n.RefId == refId)
                {
                    existingId = n.Id;
                }
            }

            // 1.1) Determine text
            if (zdroj is Book)
            {
                if (existingId == -1)
                {
                    text = "Uživatel " + user.Name  + " přidal příspěvek " + postName;
                }
                else
                {
                    text = "Uživatel upravil příspěvek";
                }
            }

            if (zdroj is KnihovnaUser)
            {
                if (existingId == -1)
                {
                    text = "Uživatel " + user.Name + " se zaregistroval";
                }
            }

            if (zdroj is KnihovnaKomentare)
            {
                existingId = -1;
                if (existingId == -1)
                {
                    text = "Uživatel " + user.Name + " přidal komentář k příspěvku " + postName;
                }
            }

            // 2) We new doesnt exist already
            if (existingId == -1)
            {
                KnihovnaNovinky novinka = new KnihovnaNovinky();
                novinka.Id = Books.Counter();
                novinka.RefId = refId;
                novinka.Date = DateTime.Now;
                novinka.Priority = priority;
                novinka.Sticky = sticky;
                novinka.Text = text;
                novinka.Type = (int) type;
                novinka.TypeSub = 0;
                novinka.UserId = userId;
                novinka.Version = 1;
                novinka.Action = action;
                novinka.Controller = controller;

                dao.Create(novinka);
            }


        }

        public static string GetImage(KnihovnaNovinky n)
        {
            if (n.Type == (int)typeToInt.Book)
            {
                return "post_new.png";
            }
            if (n.Type == (int)typeToInt.User)
            {
                return "user_new.png";
            }
            if (n.Type == (int)typeToInt.Comment)
            {
                return "comment_new.png";
            }

            return "user_new.png";
        }
    }
}