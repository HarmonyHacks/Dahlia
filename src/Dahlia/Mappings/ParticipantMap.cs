using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Mapping;
using Dahlia.Models;

namespace Dahlia.Mappings
{
    public class ParticipantMap : ClassMap<Participant>
    {
        public ParticipantMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.FirstName);
            Map(x => x.LastName);
            Map(x => x.RoomCode);
            Map(x => x.Notes);
            Map(x => x.DateReceived);

        }
    }
}