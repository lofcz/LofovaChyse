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

        public static void SendRateNotification(KnihovnaUser userTo, int moznost, KnihovnaUser userFrom, Book topic)
        {
            string text, desc;
            text = "";
            desc = "";
            KnihovnaUserDao dap = new KnihovnaUserDao();

            // Super
            if (moznost == 0)
            {
                text = userFrom.Name + " dal " + "<b style=\"color: #ff5252\">Super</b>" + " tvému kometáři u příspěvku " + topic.Name;
                desc = "Reputace: <b>2</b></br>Krevity: <b>0.2</b>";
                userTo.Reputation += 2;
                userTo.Money += 0.2;
            }

            // Souhlasím
            if (moznost == 1)
            {
                text = userFrom.Name + " dal " + "<b style=\"color: #bbebff\">Souhlasím</b>" + " tvému kometáři u příspěvku " + topic.Name;
                desc = "Zkušenosti: <b>2</b></br>Krevity: <b>0.2</b>";
                userTo.Money += 0.2;
                userTo.Exp += 2;
            }

            // To mě mrzí
            if (moznost == 2)
            {
                text = userFrom.Name + " dal " + "<b style=\"color: #86d673\">To mě mrzí</b>" + " tvému kometáři u příspěvku " + topic.Name;
                desc = "Reputace: <b>2</b>";
                userTo.Reputation += 2;
            }

            // Užitečné
            if (moznost == 3)
            {
                text = userFrom.Name + " dal " + "<b style=\"color: #a9a0e3\">Užitečné</b>" + " tvému kometáři u příspěvku " + topic.Name;
                desc = "Krevity: <b>0.4</b>";
                userTo.Money += 0.4;
            }

            SendNotification(text, 0, userTo.Id, "Získal jsi:</br><hr></hr>" + desc);
        }
    }
}