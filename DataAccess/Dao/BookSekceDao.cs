using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;
using NHibernate.Criterion;

namespace DataAccess.Dao
{
    public class BookSekceDao : DaoBase<BookSekce>
    {
        public IList<BookSekce> GetCategories(int? id)
        {
            return session.CreateCriteria<BookSekce>().AddOrder(Order.Asc("Id")).Add(Restrictions.Like("ParentId", id))
                .List<BookSekce>();
        }

        public IList<BookSekce> GetCategoriesDebug(int? id)
        {
            return session.CreateCriteria<BookSekce>().AddOrder(Order.Desc("Id")).Add(Restrictions.Like("ParentId", id))
                .List<BookSekce>();
        }

        public BookSekce GetbyIdNull(int? id)
        {
            return session.CreateCriteria<BookSekce>().Add(Restrictions.Eq("Id", id)).UniqueResult<BookSekce>();
        }

        public BookSekce GetParentSekce(int id)
        {
            BookSekce b = GetbyId(id);

            return session.CreateCriteria<BookSekce>().Add(Restrictions.Eq("Id", b.ParentId)).UniqueResult<BookSekce>();
        }
    }
}