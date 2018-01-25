using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Cfg;

namespace DataAccess
{
    public class NHibernateHelper
    {
        // Veliký objekt -> singleton
        private static ISessionFactory _factory;

        public static ISession Session
        {
            get
            {
                if (_factory == null)
                {
                    var cfg = new Configuration();
                    _factory = cfg.Configure(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "hibernate.cfg.xml")).BuildSessionFactory();

                }

                return _factory.OpenSession();
            }
        }
    }
}
