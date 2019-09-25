using NUnit.Framework;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Tool.hbm2ddl;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using UsingFluentNHibernate.Entities;
using UsingFluentNHibernate.Mapping;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace UsingFluentNHibernate.Test
{
    public class Tests
    {
        ISessionFactory sessionFactory;
        const string dbFile = "sample.db";
        IDictionary<string, Store> stores;
        IDictionary<string, Product> products;
        IDictionary<string, Employee> employees;
        
        [OneTimeSetUp]
        public void Setup()
        {
            sessionFactory = CreateSessionFactory();

            Assert.IsNotNull(sessionFactory);

            PrepareTestData();
        }

        void PrepareTestData()
        {
            stores = new Dictionary<string, Store>();
            products = new Dictionary<string, Product>();
            employees = new Dictionary<string, Employee>();

            stores.Add("Bargin Basin", new Store { Name = "Bargin Basin" });
            stores.Add("SuperMart", new Store { Name = "SuperMart" });

            products.Add("Potatoes", new Product { Name = "Potatoes", Price = 3.60 });
            products.Add("Fish", new Product { Name = "Fish", Price = 4.49 });
            products.Add("Milk", new Product { Name = "Milk", Price = 0.79 });
            products.Add("Bread", new Product { Name = "Bread", Price = 1.29 });
            products.Add("Cheese", new Product { Name = "Cheese", Price = 2.10 });
            products.Add("Waffles", new Product { Name = "Waffles", Price = 2.41 });

            employees.Add("Daisy", new Employee { FirstName = "Daisy", LastName = "Harrison" });
            employees.Add("Jack", new Employee { FirstName = "Jack", LastName = "Torrance" });
            employees.Add("Sue", new Employee { FirstName = "Sue", LastName = "Walkters" });
            employees.Add("Bill", new Employee { FirstName = "Bill", LastName = "Taft" });
            employees.Add("Joan", new Employee { FirstName = "Joan", LastName = "Pope" });
        }

        [Test]
        public void Test()
        {
            using (var session = sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    AddProductsToStore(stores["Bargin Basin"],
                        products["Potatoes"],
                        products["Fish"],
                        products["Bread"],
                        products["Cheese"]);
                    AddProductsToStore(stores["SuperMart"],
                        products["Bread"],
                        products["Cheese"],
                        products["Waffles"]);

                    AddEmployeesToStore(stores["Bargin Basin"],
                        employees["Daisy"],
                        employees["Jack"],
                        employees["Sue"]);
                    AddEmployeesToStore(stores["SuperMart"],
                        employees["Bill"],
                        employees["Joan"]);

                    foreach (var store in stores)
                    {
                        session.SaveOrUpdate(store.Value);
                    }

                    transaction.Commit();
                }

                using (session.BeginTransaction())
                {
                    var fromDB = session.CreateCriteria(typeof(Store))
                      .List<Store>();

                    foreach (var store in fromDB)
                    {
                        Assert.IsTrue(stores.ContainsKey(store.Name));

                        foreach (var product in store.Products)
                        {
                            Assert.IsTrue(products.ContainsKey(product.Name));
                        }
                        foreach (var staff in store.Staff)
                        {
                            Assert.IsTrue(employees.ContainsKey(staff.FirstName));
                        }
                    }
                }
            }

            var st = GetStore("SuperMart");
        }

        void AddProductsToStore(Store store, params Product[] products)
        {
            foreach (var product in products)
            {
                store.AddProduct(product);
            }
        }

        void AddEmployeesToStore(Store store, params Employee[] employees)
        {
            foreach (var employee in employees)
            {
                store.AddEmployee(employee);
            }
        }

        Store GetStore(string name)
        {
            using (var session = sessionFactory.OpenSession())
            {
                var cret = session.CreateCriteria<Store>()
                                   .Add(Restrictions.Eq("Name", name));
                var store = cret.UniqueResult<Store>();

                if (store != null)
                {
                    var p = store.Products.Where(x => x.Name == "Bread").FirstOrDefault();
                }

                return store;
            }
        }

        ISessionFactory CreateSessionFactory()
        {
            return Fluently.Configure()
                           .Database(SQLiteConfiguration.Standard.UsingFile(dbFile))
                           .Mappings(m => m.FluentMappings.AddFromAssemblyOf<MappingSet>())
                           .ExposeConfiguration(BuildSchema)
                           .BuildSessionFactory();
        }

        void BuildSchema(NHibernate.Cfg.Configuration config)
        {
            if (File.Exists(dbFile))
            {
                File.Delete(dbFile);
            }

            new SchemaExport(config).Create(false, true);
        }
    }
}
