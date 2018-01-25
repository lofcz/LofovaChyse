using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;
using NHibernate.Criterion;

namespace DataAccess.Dao
{
    public class BookPaymentDao : DaoBase<BookPayment>
    {
        public IList<BookPayment> GetUserPayments(int id)
        {
            return session.CreateCriteria<BookPayment>().Add(Restrictions.Like("UserId", id)).List<BookPayment>();
        }
    }
}
