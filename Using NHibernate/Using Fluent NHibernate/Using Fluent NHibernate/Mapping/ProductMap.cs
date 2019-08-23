using FluentNHibernate.Mapping;
using UsingFluentNHibernate.Entities;

namespace UsingFluentNHibernate.Mapping
{
    public partial class MappingSet
    {
        public class ProductMap : ClassMap<Product>
        {
            public ProductMap()
            {
                Id(x => x.Id);
                Map(x => x.Name);
                Map(x => x.Price);
                /*
                  Set up bidirectional many-to-many relationship with Store
                 */
                HasManyToMany(x => x.StoresStokedIn)
                    .Cascade.All()
                    .Inverse()
                    .Table("StoreProduct");
            }
        }
    }
}
