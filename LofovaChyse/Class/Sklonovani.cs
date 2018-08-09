using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LofovaChyse.Class
{
    public class Sklonovani
    {
        public static string Sklonuj(string slovo, int pocet)
        {
            if (slovo == "odpovedi")
            {
                if (pocet == 1)
                {
                    return "odpověď";
                }
                else if (pocet < 5)
                {
                    return "odpovědi";
                }
                else
                {
                    return "odpovědí";
                }
                
            }

            if (slovo == "starší")
            {
                if (pocet < 5)
                {
                    return "starší";
                }
                else
                {
                    return "starších";
                }
            }

            if (slovo == "navsteva")
            {
                if (pocet < 1)
                {
                    return "návštěv";
                }
                else if (pocet < 2)
                {
                    return "návštěva";
                }
                else if (pocet < 5)
                {
                    return "návštěvy";
                }
                else
                {
                    return "návštěv";
                }
            }
            return "SKLONOVANI-UNDEFINIED";
        }
    }
}