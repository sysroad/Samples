using NHibernate;
using NHibernate.Cfg;
using NHibernateBasic.Domain;

namespace NHibernateBasic.Repository
{
    public class NHibernateHelper
    {
        private static ISessionFactory sessionFactory;
        private static ISessionFactory SessionFactory
        {
            get
            {
                if (sessionFactory == null)
                {
                    var configuration = new Configuration();
                    configuration.Configure();
                    configuration.AddAssembly(typeof(Product).Assembly);
                    sessionFactory = configuration.BuildSessionFactory();
                }
                return sessionFactory;
            }
        }

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }
    }
}
