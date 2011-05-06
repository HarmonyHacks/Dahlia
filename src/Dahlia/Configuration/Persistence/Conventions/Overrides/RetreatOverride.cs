using Dahlia.Models;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace Dahlia.Configuration.Persistence.Conventions.Overrides
{
    public class RetreatOverride : IAutoMappingOverride<Retreat>
    {
        public void Override(AutoMapping<Retreat> mapping)
        {
            mapping.IgnoreProperty(x => x.IsFull);
            mapping.IgnoreProperty(x => x.AssignedBeds);
        }
    }
}