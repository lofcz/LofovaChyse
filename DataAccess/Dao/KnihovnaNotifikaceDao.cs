using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;
using NHibernate.Criterion;

namespace DataAccess.Dao
{
    public class KnihovnaNotifikaceDao : DaoBase<KnihovnaNotifikace>
    {
        public IList<KnihovnaNotifikace> GetUserNotifikace(int id)
        {
            return session.CreateCriteria<KnihovnaNotifikace>().Add(Restrictions.Like("UserTo", id)).List<KnihovnaNotifikace>();
        }
    }
}
