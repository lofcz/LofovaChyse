using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;
using NHibernate.Criterion;

namespace DataAccess.Dao
{
    public class KnihovnaPrateleDao : DaoBase<KnihovnaPratele>
    {
        public bool GetFriendship(int id1, int id2)
        {
            int total = session.CreateCriteria<KnihovnaPratele>().Add(Restrictions.Like("UserFrom", id1)).Add(Restrictions.Like("UserTo", id2)).SetProjection(Projections.RowCount()).UniqueResult<int>();
            int total2 = session.CreateCriteria<KnihovnaPratele>().Add(Restrictions.Like("UserTo", id2)).Add(Restrictions.Like("UserFrom", id1)).SetProjection(Projections.RowCount()).UniqueResult<int>();

            if (total2 + total > 0)
            {
                return true;
            }

            return false;
        }

        public bool GetFriendshipConfirmed(int id1, int id2)
        {
            int total = session.CreateCriteria<KnihovnaPratele>().Add(Restrictions.Like("UserFrom", id1)).Add(Restrictions.Like("UserTo", id2)).Add(Restrictions.Like("Accepted", true)).SetProjection(Projections.RowCount()).UniqueResult<int>();
            int total2 = session.CreateCriteria<KnihovnaPratele>().Add(Restrictions.Like("UserTo", id2)).Add(Restrictions.Like("UserFrom", id1)).Add(Restrictions.Like("Accepted", true)).SetProjection(Projections.RowCount()).UniqueResult<int>();

            if (total2 + total > 0)
            {
                return true;
            }

            return false;
        }
    }

}
