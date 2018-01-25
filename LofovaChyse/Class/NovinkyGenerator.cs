﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataAccess.Dao;
using DataAccess.Models;

namespace LofovaChyse.Class
{
    public class NovinkyGenerator
    {
        public enum typeToInt
        {
            Undefined,
            Book
        }

        public static void PridatNovinku<T>(T zdroj, int userId, int priority = 0, bool sticky = false)
        {
            typeToInt type = typeToInt.Undefined;
            int typeSub = 0;
            int refId = 0;
            int existingId = -1;
            string text = "";
            string postName = "";

            KnihovnaNovinkyDao dao = new KnihovnaNovinkyDao();
            KnihovnaUser user = new KnihovnaUserDao().GetbyId(userId);

            if (zdroj is Book)
            {
                type = typeToInt.Book;
                refId = (zdroj as Book).Id;
                postName = (zdroj as Book).Name;
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
                    text = "Uživatel " + user.Name  + " přidal příspěvek" + postName;
                }
                else
                {
                    text = "Uživatel upravil příspěvek";
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

                dao.Create(novinka);
            }


        }

    }
}