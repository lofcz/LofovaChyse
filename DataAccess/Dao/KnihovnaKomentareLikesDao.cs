using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;
using NHibernate.Criterion;

namespace DataAccess.Dao
{
    public class KnihovnaKomentareLikesDao : DaoBase<KnihovnaKomentareLikes>
    {
        public KnihovnaKomentareLikesDao() : base()
        {

        }

        public int? GetComentLikes(int id, int typ)
        {
            int total = session.CreateCriteria<KnihovnaKomentareLikes>().Add(Restrictions.Like("ComentId", id)).Add(Restrictions.Like("Value", typ)).SetProjection(Projections.RowCount()).UniqueResult<int>();
            return total;
        }

    }
}
