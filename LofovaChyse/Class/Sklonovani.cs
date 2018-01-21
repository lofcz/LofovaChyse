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
            return "SKLONOVANI-UNDEFINIED";
        }
    }
}