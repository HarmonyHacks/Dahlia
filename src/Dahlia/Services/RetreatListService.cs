using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dahlia.Services
{
    public class RetreatSummary
    {
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public Uri AddParticipantLink { get; set; }
    }

    public interface IRetreatListService
    {
        IEnumerable<RetreatSummary> List();
    }

    public class RetreatListService
    {
    }
}