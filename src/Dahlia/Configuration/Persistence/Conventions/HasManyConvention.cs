using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace Dahlia.Configuration.Persistence.Conventions
{
    public class HasManyConvention : IHasManyConvention 
    {
        public void Apply(IOneToManyCollectionInstance instance)
        {
            instance.Access.CamelCaseField(CamelCasePrefix.Underscore);
            instance.Access.ReadOnlyPropertyThroughCamelCaseField(CamelCasePrefix.Underscore);
            instance.Cascade.All();
        }
    }
}