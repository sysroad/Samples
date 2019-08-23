using NUnit.Framework;
using NHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using UsingFluentNHibernate.Entities;
using UsingFluentNHibernate.Mapping;

namespace UsingFluentNHibernate.Test
{
    public class Tests
    {
        [Test]
        public void Test()
        {
            var sessionFactory = CreateSessionFactory();

            Assert.IsNotNull(sessionFactory);

            using (var session = sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {

            }
        }

        ISessionFactory CreateSessionFactory()
        {
            return Fluently.Configure()
                           .Database(SQLiteConfiguration.Standard.UsingFile("sample.db"))
                           .Mappings(m => m.FluentMappings.AddFromAssemblyOf<MappingSet>())
                           .BuildSessionFactory();
                           
        }
    }
}
