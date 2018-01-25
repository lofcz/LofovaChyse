using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;
using NHibernate.Criterion;

namespace DataAccess.Dao
{
    public class KnihovnaZedDao : DaoBase<KnihovnaZed>
    {
        public IList<KnihovnaZed> GetUserWall(int id)
        {
            return session.CreateCriteria<KnihovnaZed>().Add(Restrictions.Like("PostOwner.Id", id)).List<KnihovnaZed>();
        }
    }
}
