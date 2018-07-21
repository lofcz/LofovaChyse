using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataAccess.Dao;
using DataAccess.Models;

namespace LofovaChyse.Class
{
    public class LevelUp
    {
        // <exp, rep>
        public static int[,] Pozadavky = new int[8, 5];
        public static string[] LevelUpTeaser = new string[10];

        static LevelUp()
        {
            // A
            Pozadavky[0, 0] = 10;
            Pozadavky[0, 1] = 1;

            // B
            Pozadavky[1, 0] = 50;
            Pozadavky[1, 1] = 15;

            // C
            Pozadavky[2, 0] = 120;
            Pozadavky[2, 1] = 40;

            // D 
            Pozadavky[3, 0] = 250;
            Pozadavky[3, 1] = 100;

            // E
            Pozadavky[4, 0] = 500;
            Pozadavky[4, 1] = 200;

            // F
            Pozadavky[5, 0] = 1000;
            Pozadavky[5, 1] = 500;


            // String pro level up teaser:
            LevelUpTeaser[0] = "přístup ke své zdi, galerii, snippetu a soundUpu";                                  // A
            LevelUpTeaser[1] = "možnost navrhovat úpravy pro články";
            LevelUpTeaser[2] = "něco C";
            LevelUpTeaser[3] = "něco D";
            LevelUpTeaser[4] = "něco E";
            LevelUpTeaser[5] = "něco F";
        }

        public static string GetNeededStats(string login)
        {
            string r = "[chyba v získání potřebných zkušeností";

            KnihovnaUserDao d = new KnihovnaUserDao();
            KnihovnaUser u = d.GetByLogin(login);

            int xp = u.Exp;
            int rep = u.Reputation;
            int lvl = 0;

            // 1) Získáme level uživatele
            /*
            for (var i = 0; i < 6; i++)
            {
                if (xp >= Pozadavky[i, 0] && rep >= Pozadavky[i, 1])
                {
                    lvl++;
                }
                else
                {
                    break;
                }
            }*/

            lvl = u.AuthLevel;

            int xpDif = Pozadavky[lvl, 0] - xp;
            int repDif = Pozadavky[lvl, 1] - rep;

            string tvarXp = "zkušenost";
            string tvarRep = "bod";

            if (xpDif > 4)
            {
                tvarXp = "zkušeností";
            }
            else if (xpDif > 1)
            {
                tvarXp = "zkušenosti";
            }

            if (repDif > 1)
            {
                tvarRep = "body";
            }

            if (repDif > 4)
            {
                tvarRep = "bodů";
            }

            // Sestavíme string
            if (xp < Pozadavky[lvl, 0] && rep < Pozadavky[lvl, 1]) // V případě, že chybí oba staty
            {
                r = "Pro dosažení další úrovně ti chybí <b>" + xpDif + "</b> " + tvarXp + " a <b>" + repDif + "</b> "+ tvarRep + " reputace.";
            }
            else if (xp < Pozadavky[lvl, 0]) // Pokud chybí pouze xp
            {
                r = "Pro dosažení další úrovně ti chybí <b>" + xpDif + "</b> "+ tvarXp + ".";
            }
            else // Pokud chybí pouze reputace
            {
                r = "Pro dosažení další úrovně ti chybí <b>" + repDif + "</b>" + tvarRep + " reputace.";
            }

            // Přidáme získané výhody
            r += "<br/><small>Na další úrovni získáš " + LevelUpTeaser[lvl] + ".</small>";


            // Uživatel má max lvl
            if (lvl >= 5)
            {
                r = "";
            }

            return r;
        }


    }
}