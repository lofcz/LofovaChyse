using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;
using NHibernate.Criterion;

namespace DataAccess.Dao
{
    public class KnihovnaKomentareDao : DaoBase<KnihovnaKomentare>
    {
        public KnihovnaKomentareDao() : base()
        {

        }

        public IList<KnihovnaKomentare> GetCommentSubcomments(int id)
        {
            return session.CreateCriteria<KnihovnaKomentare>().Add(Restrictions.Like("ReplyId", id)).List<KnihovnaKomentare>();
        }
    }
}
