using FluentNHibernate.Mapping;
using UsingFluentNHibernate.Entities;

namespace UsingFluentNHibernate.Mapping
{
    public partial class MappingSet
    {
        public class StoreMap : ClassMap<Store>
        {
            public StoreMap()
            {
                Id(x => x.Id);
                Map(x => x.Name);
                HasMany(x => x.Staff)
                    /* Inverse :
                      이 요소와 관계가 있는 쪽에서 저장 의무가 있음.
                      Store.AddEmployee(Employee employee)
                      >> employee.Store = this;
                    */
                    .Inverse()
                    /* Cascade.All :                  
                      Cascade events down to the entities
                      Store 를 저장하면 Staff 들도 모두 저장됨.
                    */
                    .Cascade.All();
                HasManyToMany(x => x.Products)
                    /* Cascade.All :
                      Store 를 저장하면 Product 모두 저장됨.
                     */
                    .Cascade.All()
                    /* Table
                      many-to-many 조인에 사용되는 테이블명을 지정
                      양방향 many-to-many 접근에 대해서만 필요하고,
                      그외의 경우에는 필요하지 않음.
                     */
                    .Table("StoreProduct");
            }
        }
    }
}
