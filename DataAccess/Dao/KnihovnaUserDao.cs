using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;
using NHibernate.Criterion;

namespace DataAccess.Dao
{
    public class KnihovnaUserDao : DaoBase<KnihovnaUser>
    {
        // Existuje Conjunction / Disjunction objekty pro složené podmínky

        public KnihovnaUser GetByLoginAndPassword(string login, string password)
        {
            return session.CreateCriteria<KnihovnaUser>().Add(Restrictions.Eq("Login", login)).Add(Restrictions.Eq("Password", password)).UniqueResult<KnihovnaUser>();
        }

        public KnihovnaUser GetByLogin(string login)
        {
            return session.CreateCriteria<KnihovnaUser>().Add(Restrictions.Eq("Login", login)).UniqueResult<KnihovnaUser>();
        }
    }
}
