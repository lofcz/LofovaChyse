using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using DataAccess.Dao;
using DataAccess.Models;
using LofovaChyse.Controllers;

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
                "Proč nemá c++ garbage collector? Nic by nezůstalo.",
                "Vyje Likandro na měsíc?"
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

            return z ?? 0;
        }

        public static int GetBookUserRating(int id, int userId)
        {
            BookLikesDao d = new BookLikesDao();
            int k = d.GetBookUserLike(id, userId);

            return k;
        }

        public static string GetBookPath(int id)
        {
            // 1) Reverzně určíme cestu
            // --> Vezmeme aktuální kategorii příspěvku
            // --> Najdeme kategorii, která má jako potomka tuto kategorii
            // --> Opakujeme dokud p != -1
            // --> Na každé iteraci stavíme aktuální část cesty

            string outputString = "";

            BookDao d = new BookDao();
            BookSekceDao sekceDao = new BookSekceDao();

            Book prispevek = d.GetbyId(id);
            bool dalsiIterace = true;

            BookSekce aktualniSekce = sekceDao.GetbyIdNull(prispevek.SectionId);
            outputString += aktualniSekce.DebugName + "/ ";

            while (dalsiIterace)
            {                       
                // Najdeme potomka sekce
                aktualniSekce = sekceDao.GetParentSekce(aktualniSekce.Id);
                outputString += aktualniSekce.DebugName + "/";
                if (aktualniSekce.ParentId <= 0)
                {
                    dalsiIterace = false;
                }
            }

            // 2) Teď máme cestu, ale reverzně
            // --> Konec cesty / Střed cesty / Začátek
            // --> Potřebujeme: Začátek / Střed / Konec
            // --> Rozdělíme string na tokeny
            // --> Reverzně dáme tokeny do správného pořadí

            string[] tokeny = outputString.Split('/');
            outputString = "";

            for (var i = tokeny.Length - 1; i >= 0; i--)
            {
                outputString += tokeny[i];

                // Vizuálně oddělujeme části cesty
                if (i > 0 && i != tokeny.Length - 1)
                {
                    outputString += " / ";
                }
            }

            return outputString;
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

        public static List<BookOdznakArchetyp> GetAllOdznaky()
        {
            BookOdznakArchetypDao dao = new BookOdznakArchetypDao();
            List<BookOdznakArchetyp> list = dao.GetAll() as List<BookOdznakArchetyp>;
            return list;
        }

        public static List<KnihovnaOceneni> GetUserOceneni(string username)
        {
            KnihovnaOceneniDao d = new KnihovnaOceneniDao();
            List<KnihovnaOceneni> l = d.GetUserAchievements(new KnihovnaUserDao().GetByLogin(username).Id) as List<KnihovnaOceneni>;

            return l;
        }

        public static List<BookOdznak> GetBookOceneni(int id)
        {
            BookOdznakDao d = new BookOdznakDao();
            return d.GetBookOceneni(id) as List<BookOdznak>;
        }

        public static bool UsersAreFrineds(string name1, string name2)
        {
            KnihovnaUserDao d = new KnihovnaUserDao();
            KnihovnaUser user1 = d.GetByLogin(name1);
            KnihovnaUser user2 = d.GetByLogin(name2);

            KnihovnaPrateleDao pd = new KnihovnaPrateleDao();
            bool pratele = pd.GetFriendship(user1.Id, user2.Id);

            return pratele;
        }

        public static bool UsersAreFriendsConfirmed(string name1, string name2)
        {
            KnihovnaUserDao d = new KnihovnaUserDao();
            KnihovnaUser user1 = d.GetByLogin(name1);
            KnihovnaUser user2 = d.GetByLogin(name2);

            KnihovnaPrateleDao pd = new KnihovnaPrateleDao();
           // pd.

            bool pratele = pd.GetFriendshipConfirmed(user1.Id, user2.Id);

            return pratele;
        }

        public static object Test()
        {
            // Hello
            return null;
        }

        public static void ConfirmFriendShip(int friendID)
        {
            KnihovnaPrateleDao d = new KnihovnaPrateleDao();
            KnihovnaPratele p = d.GetbyId(friendID);

            p.Accepted = true;
            p.DateAccepted = DateTime.Now;
            d.Update(p);
        }

        public static List<KnihovnaUserRole> GetUserRoles(string login)
        {
            KnihovnaUserDao d = new KnihovnaUserDao();
            KnihovnaUser u = d.GetByLogin(login);
            KnihovnaUserRoleDao dd = new KnihovnaUserRoleDao();

            List<KnihovnaUserRole> role = dd.GetUserRoles(u.Id) as List<KnihovnaUserRole>;
            return role;
        }

        public static string UserRoleName(KnihovnaUserRole r)
        {
            string s = "";

            if (r.RoleId == 1)
            {
                s = "Admin";    
            }

            if (r.RoleId == 2)
            {
                s = "Uživatel";
            }

            return s;
        }

        public static KnihovnaUser GetUser(string name)
        {
            KnihovnaUserDao dao = new KnihovnaUserDao();
            KnihovnaUser user = dao.GetByLogin(name);
            
                return user;
        }

        public static bool IsUserBanned(string name)
        {
            KnihovnaUserDao knihovnaUserDao = new KnihovnaUserDao();
            KnihovnaUser knihovnaUser = knihovnaUserDao.GetByLogin(name);
		    DateTime restrictedTo = knihovnaUser.RestrictedTo;

            if (DateTime.Compare(restrictedTo, DateTime.Now) >= 0)
            {
                return true;
            }

            return false;
        }

        public static DateTime UserBannedUntil(string name)
        {
            KnihovnaUserDao knihovnaUserDao = new KnihovnaUserDao();
            KnihovnaUser knihovnaUser = knihovnaUserDao.GetByLogin(name);
            DateTime restrictedTo = knihovnaUser.RestrictedTo;
            return restrictedTo;
        }

        public static KnihovnaUser GetUserById(int userId)
        {
            KnihovnaUser k = new KnihovnaUserDao().GetbyId(userId);
            return k;
        }

        public static Book GetBook(int bookId)
        {
            Book b = new BookDao().GetbyId(bookId);
            return b;
        }

        public static string VoucherToString(int type)
        {
            if (type == 0)
            {
                return "Krevity x10";
            }

            return "[UNDEFINED-VOUCHER-GENERAL]";
        }

        [Authorize]
        public static string RestoreChat(string name)
        {
            string s = "";

            KnihovnaUserDao ud = new KnihovnaUserDao();
            ChatZpravyDao d = new ChatZpravyDao();
            List<ChatZpravy> l = d.GetAll() as List<ChatZpravy>;
            KnihovnaUser cu = ud.GetByLogin(name);

            foreach (ChatZpravy z in l)
            {
                KnihovnaUser u = ud.GetbyId(z.UserFrom);

                // Zpráva od teve
                if (u.Id == cu.Id)
                {
                    s += "<span class='float-right chatMsgYour'>" + z.Text +"</span><br/>";
                }
                else
                {
                    s += "<img src='" + ("Uploads/KnihovnaUzivatele/" + GetMiniaturePicture(u.Login)) + "' /> <span class='chatMsgHis'>" + z.Text + "</span><br/>";
                }
                
            }

            return s;
        }
    }
}