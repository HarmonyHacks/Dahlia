using Dahlia.Models;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace Dahlia.Conventions.Overrides
{
    public class RegistrationOverride : IAutoMappingOverride<Registration>
    {
        public void Override(AutoMapping<Registration> mapping)
        {
            mapping.HasOne(x => x.Participant).Cascade.All();
        }
    }
}