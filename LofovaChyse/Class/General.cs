using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataAccess.Dao;
using DataAccess.Models;

namespace LofovaChyse.Class
{
    public class General
    {
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

        public static string GetCurrentUserNotifications(string name)
        {
            KnihovnaUserDao dao = new KnihovnaUserDao();
            KnihovnaUser user = dao.GetByLogin(name);

            KnihovnaNotifikaceDao nDao = new KnihovnaNotifikaceDao();
            IList<KnihovnaNotifikace> list = nDao.GetUserNotifikace(user.Id);

            return list.Count.ToString();
        }

        public static IList<KnihovnaNotifikace> GetCurrentUserNotificationsObject(string name)
        {
            KnihovnaUserDao dao = new KnihovnaUserDao();
            KnihovnaUser user = dao.GetByLogin(name);

            KnihovnaNotifikaceDao nDao = new KnihovnaNotifikaceDao();
            IList<KnihovnaNotifikace> list = nDao.GetUserNotifikace(user.Id);

            return list;
        }

        public static string GetMiniaturePicture(string name)
        {
            KnihovnaUserDao dao = new KnihovnaUserDao();
            KnihovnaUser user = dao.GetByLogin(name);
            var i = user.ImageName;

            return user.ImageName;
        }
    }
}