using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;
using NHibernate.Criterion;

namespace DataAccess.Dao
{
    public class BookOdznakDao : DaoBase<BookOdznak>
    {
        public IList<BookOdznak> GetBookOceneni(int id)
        {
            return session.CreateCriteria<BookOdznak>().Add(Restrictions.Like("BookId", id)).List<BookOdznak>();
        }
    }
}
