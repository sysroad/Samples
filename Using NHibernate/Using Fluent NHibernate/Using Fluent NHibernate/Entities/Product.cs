using System;
using System.Collections.Generic;
using System.Text;

namespace UsingFluentNHibernate.Entities
{
    public class Product
    {
        public virtual int Id { get; protected set; }
        public virtual string Name { get; set; }
        public virtual double Price { get; set; }
        public virtual IList<Store> StoresStokedIn { get; protected set; }

        public Product()
        {
            StoresStokedIn = new List<Store>();
        }
    }
}
