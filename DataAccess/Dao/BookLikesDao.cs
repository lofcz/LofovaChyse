using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;
using NHibernate.Criterion;

namespace DataAccess.Dao
{
    public class BookLikesDao : DaoBase<BookLikes>
    {
        public BookLikesDao() : base()
        {

        }

        public int? GetBookLikes(int id, int typ)
        {
            int total = session.CreateCriteria<BookLikes>().Add(Restrictions.Like("BookId", id)).Add(Restrictions.Like("Value", typ)).SetProjection(Projections.RowCount()).UniqueResult<int>();
            return total;
        }

        public int? GetBookLikesAll(int id)
        {
            int total = session.CreateCriteria<BookLikes>().Add(Restrictions.Like("BookId", id)).SetProjection(Projections.RowCount()).UniqueResult<int>();
            return total;
        }

        public int GetBookUserLike(int id, int userId)
        {
            IList<BookLikes> total = session.CreateCriteria<BookLikes>().Add(Restrictions.Like("BookId", id)).Add(Restrictions.Like("UserId", id)).List<BookLikes>();
            if (total.Count > 0)
            {
                return total[0].Value;
            }

            return -1;

        }
    }
}
