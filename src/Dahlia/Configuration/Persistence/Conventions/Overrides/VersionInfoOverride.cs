using System;
using Dahlia.Models;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace Dahlia.Configuration.Persistence.Conventions.Overrides
{
    public class VersionInfoOverride : IAutoMappingOverride<VersionInfo>
    {
        public void Override(AutoMapping<VersionInfo> mapping)
        {
            mapping.Id(x => x.Version).GeneratedBy.Assigned();
        }
    }
}