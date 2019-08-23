using System;
using System.Collections.Generic;
using NHibernate.Criterion;
using NHibernateBasic.Domain;

namespace NHibernateBasic.Repository
{
    public interface IProductRepository
    {
        void Add(Product product);
        void Update(Product product);
        void Remove(Product product);
        Product GetById(Guid productId);
        Product GetByName(string productName);
        ICollection<Product> GetByCategory(string productCategory);
    }

    public class ProductRepository : IProductRepository
    {
        public void Add(Product product)
        {
            using (var session = NHibernateHelper.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                session.Save(product);
                transaction.Commit();
            }
        }

        public ICollection<Product> GetByCategory(string productCategory)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                var products = session.CreateCriteria<Product>()
                                      .Add(Restrictions.Eq("Category", productCategory))
                                      .List<Product>();
                return products;
            }
        }

        public Product GetById(Guid productId)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                return session.Get<Product>(productId);
            }
        }

        public Product GetByName(string productName)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                var product = session.CreateCriteria<Product>()
                                     .Add(Restrictions.Eq("Name", productName))
                                     .UniqueResult<Product>();
                return product;
            }
        }

        public void Remove(Product product)
        {
            using (var session = NHibernateHelper.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                session.Delete(product);
                transaction.Commit();
            }
        }

        public void Update(Product product)
        {
            using (var session = NHibernateHelper.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                session.Update(product);
                transaction.Commit();
            }
        }
    }
}
