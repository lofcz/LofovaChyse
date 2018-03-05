using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;
using NHibernate.Criterion;

namespace DataAccess.Dao
{
    public class KnihovnaNovinkyDao : DaoBase<KnihovnaNovinky>
    {
        public IList<KnihovnaNovinky> GetNovinkyWhereType(int type)
        {
            return session.CreateCriteria<KnihovnaNovinky>().Add(Restrictions.Like("Type", type)).List<KnihovnaNovinky>();
        }
    }
}
