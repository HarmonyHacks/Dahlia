using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Dahlia.Configuration.Persistence.Conventions
{
    public class HasManyConvention : IHasManyConvention 
    {
        public void Apply(IOneToManyCollectionInstance instance)
        {
            instance.Cascade.All();
        }
    }
}