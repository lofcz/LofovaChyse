using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;
using NHibernate.Criterion;

namespace DataAccess.Dao
{
    public class KnihovnaOceneniDao : DaoBase<KnihovnaOceneni>
    {
        public IList<KnihovnaOceneni> GetUserAchievements(int id)
        {
            return session.CreateCriteria<KnihovnaOceneni>().Add(Restrictions.Like("UserId", id)).List<KnihovnaOceneni>();
        }
    }
}
