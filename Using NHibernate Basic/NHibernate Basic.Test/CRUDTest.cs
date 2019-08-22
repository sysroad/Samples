using NUnit.Framework;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NHibernateBasic.Repository;
using NHibernateBasic.Domain;

namespace NHibernateBasic.Test
{
    [TestFixture]
    public class ProductRepositoryFixture
    {
        private ISessionFactory sessionFactory;
        private Configuration configuration;

        [OneTimeSetUp]
        public void ClassInit()
        {
            configuration = new Configuration();
            configuration.Configure();
            configuration.AddAssembly(typeof(Product).Assembly);
            sessionFactory = configuration.BuildSessionFactory();
        }

        [SetUp]
        public void Setup()
        {
            new SchemaExport(configuration).Execute(false, true, false);
        }

        [Test]
        public void CanAddNewProduct()
        {
            var product = new Product { Name = "Apple", Category = "Fruits" };
            IProductRepository repository = new ProductRepository();
            repository.Add(product);

            using (var session = sessionFactory.OpenSession())
            {
                var fromDB = session.Get<Product>(product.Id);
                Assert.IsNotNull(fromDB);
                Assert.AreNotSame(product, fromDB);
                Assert.AreEqual(product.Name, fromDB.Name);
                Assert.AreEqual(product.Category, fromDB.Category);
            }
        }
    }
}
