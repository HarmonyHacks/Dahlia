using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Mapping;
using Dahlia.Models;

namespace Dahlia.Mappings
{
    public class RetreatMap : ClassMap<Retreat>
    {
        public RetreatMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.StartDate);
            Map(x => x.Description);
        }
    }
}