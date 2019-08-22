using System;
using System.Collections.Generic;
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
            throw new NotImplementedException();
        }

        public Product GetById(Guid productId)
        {
            throw new NotImplementedException();
        }

        public Product GetByName(string productName)
        {
            throw new NotImplementedException();
        }

        public void Remove(Product product)
        {
            throw new NotImplementedException();
        }

        public void Update(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
