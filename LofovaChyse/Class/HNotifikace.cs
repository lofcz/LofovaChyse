using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataAccess.Dao;
using DataAccess.Models;

namespace LofovaChyse.Class
{
    public class HNotifikace
    {
        enum e_rewards
        {
            krevit10
        }

        public static string GetFrom(int id)
        {
            if (id == -1)
            {
                return "systém";
            }

            KnihovnaUserDao dao = new KnihovnaUserDao();
            KnihovnaUser user = dao.GetbyId(id);

            return user.Name;
        }

        public static string GetButton(int type)
        {
            if (type == ((int)e_rewards.krevit10) + 1)
            {
                return "Převzít 10 krevitů!";
            }

            return "Potvrdit";
        }

        public static void SendNotification(string text, int rewardType, int userTo, string popis = "")
        {
            KnihovnaNotifikace n = new KnihovnaNotifikace()
            {
                Text = text,
                DateSent = DateTime.Now,
                Displayed = false,
                ForceType = 0,
                Id = Books.Counter(),
                RewardType = rewardType,
                UserTo = userTo,
                UserFrom = -1,
                Description = popis
            };

            KnihovnaNotifikaceDao dao = new KnihovnaNotifikaceDao();
            dao.Create(n);
        }
    }
}