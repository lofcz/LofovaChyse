using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Books
    {
        private static int id = 0;

        public static int Counter()
        {
            return ++id;
        }
    }
}
