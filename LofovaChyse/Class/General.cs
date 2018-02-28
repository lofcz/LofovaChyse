using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Web;
using System.Web.Mvc;
using DataAccess.Dao;
using DataAccess.Models;

namespace LofovaChyse.Class
{
    public class General
    {
        static Random random = new Random();

        public static double GetCurrentUserMoney(string name)
        {
            KnihovnaUserDao dao = new KnihovnaUserDao();
            KnihovnaUser user = dao.GetByLogin(name);

            return user.Money;
        }

        public static string GetCurrentUserName(string name)
        {
            KnihovnaUserDao dao = new KnihovnaUserDao();
            KnihovnaUser user = dao.GetByLogin(name);

            return user.Name;
        }

        public static int GetCurrentUserNotifications(string name)
        {
            KnihovnaUserDao dao = new KnihovnaUserDao();
            KnihovnaUser user = dao.GetByLogin(name);

            KnihovnaNotifikaceDao nDao = new KnihovnaNotifikaceDao();
            IList<KnihovnaNotifikace> list = nDao.GetUserNotifikace(user.Id);

            int toReturn = 0;

            foreach (KnihovnaNotifikace n in list)
            {
                if (!n.Displayed)
                {
                    toReturn++;
                }
            }

            return toReturn;
        }

        [Authorize]
        public static int GetCurrentUserNotificationsAll(string name)
        {
            KnihovnaUserDao dao = new KnihovnaUserDao();
            KnihovnaUser user = dao.GetByLogin(name);

            KnihovnaNotifikaceDao nDao = new KnihovnaNotifikaceDao();
            IList<KnihovnaNotifikace> list = nDao.GetUserNotifikace(user.Id);

            return list.Count;
        }

        public static IList<KnihovnaNotifikace> GetCurrentUserNotificationsObject(string name, bool onlyUnread = false)
        {
            KnihovnaUserDao dao = new KnihovnaUserDao();
            KnihovnaUser user = dao.GetByLogin(name);

            KnihovnaNotifikaceDao nDao = new KnihovnaNotifikaceDao();
            IList<KnihovnaNotifikace> list = nDao.GetUserNotifikace(user.Id);
            IList<KnihovnaNotifikace> fList = new List<KnihovnaNotifikace>();

            if (onlyUnread)
            {
                foreach (KnihovnaNotifikace n in list)
                {
                    if (!n.Displayed)
                    {
                        fList.Add(n);
                    }
                }

                return fList;
            }


            return list;
        }

        public static string GetMiniaturePicture(string name)
        {
            KnihovnaUserDao dao = new KnihovnaUserDao();
            KnihovnaUser user = dao.GetByLogin(name);
            var i = user.ImageName;

            return user.ImageName;
        }

        [Authorize]
        public static bool UserUnlockedPost(int postId, string name)
        {
            KnihovnaUserDao dao = new KnihovnaUserDao();
            KnihovnaUser user = dao.GetByLogin(name);

            IList<BookPayment> list = new BookPaymentDao().GetUserPayments(user.Id);

            foreach (BookPayment payment in list)
            {
                if (payment.PostId == postId)
                {
                    return true;
                }
            }

            return false;
        }

        public static string GetFooter()
        {
            string[] options =
            {
                "S láskou vytvořil Matěj Štágl",
                "2B || !2B?",
                "K r e v e t a",
                "Secret voucher - #kr3vet4",
                "Sen, nebo realita?",
                "--> ___ <---",
                "Zdravím, pozemšťane!",
                "K večeru očekáváme byte[] stream bouři",
                "Naše servery pohání ZX Spectrum",
                "Programátor - stroj přeměňující kávu na kód",
                "Nadřazená rasa programátorů - Nerdicové",
                "Na bugfree kód jsme neměli rozpočet",
                "Knock, knock.. kdo tam? (dlouhá pauza).. Java",
                "['hip','hip']",
                "Dokumentace je jako sex. Když je špatná, je lepší než nic",
                "Abys pochopil rekurzi, musíš napřed pochopit rekurzi",
                "get; in; get; out; (repeat)",
                "[ $[ $RANDOM % 6 ] == 0 ] && rm -rf",
                "K běhu sítě je vyžadován os win XP nebo lepší (například Linux)",
                "Proč nemá c++ garbage collector? Nic by nezůstalo."
            };

            return options[random.Next(0, options.Length)];
        }

        public static List<BookSekce> GetCurrentCategories(int? currentCategory, string name)
        {
            BookSekceDao dao = new BookSekceDao();
            IList<BookSekce> list = dao.GetCategories(currentCategory);
            List<BookSekce> l = list as List<BookSekce>;

            l = l.OrderByDescending(o => o.RenderPriority).ToList();
            List<BookSekce> ll = new List<BookSekce>();

            foreach (BookSekce b in l)
            {
                if (General.AccessMatch(b.Restrikce, name))
                {
                    ll.Add(b);
                }
            }
            return ll;
        }

        public static List<string> textOutput = new List<string>();

        public static string ProcessSekce(BookSekce b, int iterace)
        {
            IList<BookSekce> list = new BookSekceDao().GetCategoriesDebug(b.Id);
            iterace++;

            foreach (BookSekce z in list)
            {
                ProcessSekce(z, iterace);
            }

            string toReturn = "";

            for (var i = 0; i < iterace; i++)
            {
                toReturn += "‌‌ ‌‌ ‌‌ ‌‌ ‌‌ ";
            }

            string tR = "";
            if (b.RenderPriority > 0)
            {
                tR = "[priority: " + b.RenderPriority.ToString() + "]";
            }

            textOutput.Add("<li>" + "<ul>" + toReturn + " > " + b.DebugName + " [id: " + b.Id.ToString() + "] " + tR + "</ul>" + "</li>");

            return "";
        }

        public static void Reverse()
        {
            textOutput.Reverse();
        }

        public static void Clean()
        {
            textOutput = new List<string>();
        }

        public static int GetBookRating(int id)
        {
            BookLikesDao d = new BookLikesDao();
            int? z = d.GetBookLikesAll(id);

            if (z.HasValue)
            {
                return (int)z;

            }

            return -1;
        }

        public static bool AccessMatch(int permLevel, string userName)
        {
            if (permLevel == 0)
            {
                return true;
            }

            KnihovnaUserDao knihovnaUserDao = new KnihovnaUserDao();
            KnihovnaUser user = knihovnaUserDao.GetByLogin(userName);

            if (user == null)
            {
                 return false;
            }

            string[] userRoles = { user.Role.Identificator };

            // Level databáze
            if (permLevel == 1)
            {
                if (userRoles.Contains("knihovnik"))
                {
                    return true;
                }

                return false;
            }

            return true;
        }

        public static List<KnihovnaOceneniArchetyp> GetAll()
        {
            KnihovnaOceneniArchetypDao dao = new KnihovnaOceneniArchetypDao();
            List<KnihovnaOceneniArchetyp> list = dao.GetAll() as List<KnihovnaOceneniArchetyp>;
            return list;
        }

        public static List<KnihovnaOceneni> GetUserOceneni(string username)
        {
            KnihovnaOceneniDao d = new KnihovnaOceneniDao();
            List<KnihovnaOceneni> l = d.GetUserAchievements(new KnihovnaUserDao().GetByLogin(username).Id) as List<KnihovnaOceneni>;

            return l;
        }

    }
}