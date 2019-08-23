using NUnit.Framework;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NHibernateBasic.Repository;
using NHibernateBasic.Domain;
using System.Collections.Generic;
using System.Linq;

namespace NHibernateBasic.Test
{
    [TestFixture]
    public class ProductRepositoryFixture
    {
        private ISessionFactory sessionFactory;
        private Configuration configuration;

        private readonly Product[] products = new[]
        {
            new Product { Name = "Melon", Category = "Fruits" },
            new Product { Name = "Pear", Category = "Fruits" },
            new Product { Name = "Milk", Category = "Beverages" },
            new Product { Name = "Coca Cola", Category = "Beverages" },
            new Product { Name = "Pepsi Cola", Category = "Beverages" },
        };

        private void CreateInitialData()
        {
            using (var session = sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var iter = products.GetEnumerator();
                while (iter.MoveNext())
                {
                    session.Save(iter.Current);
                }
                transaction.Commit();
            }
        }

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

            CreateInitialData();
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

        [Test]
        public void CanUpdateExistingProduct()
        {
            var product = products[0];
            product.Name = "Yello Pear";
            IProductRepository repository = new ProductRepository();
            repository.Update(product);

            using (var session = NHibernateHelper.OpenSession())
            {
                var fromDB = session.Get<Product>(product.Id);
                Assert.AreEqual(product.Name, fromDB.Name);
            }
        }

        [Test]
        public void CanRemoveExistingProduct()
        {
            var product = products[0];
            IProductRepository repository = new ProductRepository();
            repository.Remove(product);

            using (var session = NHibernateHelper.OpenSession())
            {
                var fromDB = session.Get<Product>(product.Id);
                Assert.IsNull(fromDB);
            }
        }

        [Test]
        public void CanGetProductById()
        {
            var product = products[1];
            IProductRepository repository = new ProductRepository();
            var fromDB = repository.GetById(product.Id);
            Assert.IsNotNull(fromDB);
            Assert.AreNotSame(product, fromDB);
            Assert.AreEqual(product.Name, fromDB.Name);
        }

        [Test]
        public void CanGetProductByName()
        {
            var product = products[1];
            IProductRepository repository = new ProductRepository();
            var fromDB = repository.GetByName(product.Name);
            Assert.IsNotNull(fromDB);
            Assert.AreNotSame(product, fromDB);
            Assert.AreEqual(product.Name, fromDB.Name);
        }

        [Test]
        public void CanGetProductsByCategory()
        {
            IProductRepository repository = new ProductRepository();
            var fromDB = repository.GetByCategory("Fruits");

            Assert.IsNotNull(fromDB);
            Assert.IsTrue(IsInCollection(products[0], fromDB));
            Assert.IsTrue(IsInCollection(products[1], fromDB));
        }

        private bool IsInCollection(Product product, ICollection<Product> fromDB)
        {
            var find = fromDB.Where(x => x.Id == product.Id).FirstOrDefault();
            return find != null;
        }
    }
}
