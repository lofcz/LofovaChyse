using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;
using NHibernate.Criterion;

namespace DataAccess.Dao
{
    public class KnihovnaUserRoleDao : DaoBase<KnihovnaUserRole>
    {
        public IList<KnihovnaUserRole> GetUserRoles(int userId)
        {
            return session.CreateCriteria<KnihovnaUserRole>().Add(Restrictions.Like("UserId", userId)).List<KnihovnaUserRole>();
        }
    }
}
