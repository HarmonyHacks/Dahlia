using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Dahlia.Configuration.Persistence.Conventions
{
    public class ReferenceConvention :  IReferenceConvention
    {
        public void Apply(IManyToOneInstance instance)
        {
            instance.Cascade.All();
        }
    }
}