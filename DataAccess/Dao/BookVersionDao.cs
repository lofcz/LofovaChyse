using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;
using NHibernate.Criterion;

namespace DataAccess.Dao
{
    public class BookVersionDao : DaoBase<BookVersion>
    {
        public IList<BookVersion> GetBookVersions(int postId)
        {
            return session.CreateCriteria<BookVersion>().Add(Restrictions.Like("PostId", postId)).List<BookVersion>();
        }
    }
}
