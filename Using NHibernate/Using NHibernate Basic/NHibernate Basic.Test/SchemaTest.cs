using NUnit.Framework;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NHibernateBasic.Domain;

namespace NHibernateBasic.Test
{
    [TestFixture]
    public class GenerateSchemaFixture
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CanGenerateSchema()
        {
            var cfg = new Configuration();
            cfg.Configure();
            cfg.AddAssembly(typeof(Product).Assembly);

            new SchemaExport(cfg).Execute(false, true, false);
        }
    }
}