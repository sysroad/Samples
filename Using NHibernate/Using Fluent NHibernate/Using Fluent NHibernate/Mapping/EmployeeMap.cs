using FluentNHibernate.Mapping;
using UsingFluentNHibernate.Entities;

namespace UsingFluentNHibernate.Mapping
{
    public partial class MappingSet
    {
        public class EmployeeMap : ClassMap<Employee>
        {
            public EmployeeMap()
            {
                // Identifier
                Id(x => x.Id);
                // Simple properties
                Map(x => x.FirstName);
                Map(x => x.LastName);
                // References, many-to-one relationship (many Employees to one Store)
                References(x => x.Store);
            }
        }
    }
}
