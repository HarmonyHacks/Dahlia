using System;
using System.Collections.Generic;

namespace Dahlia.ViewModels
{
    public class RetreatListViewModel
    {
        public IEnumerable<RetreatListRetreatViewModel> Retreats { get; set; }
        public Uri CreateLink { get; set; }
    }

    public class RetreatListRetreatViewModel
    {
        public DateTime Date { get; set; }
        public Uri AddParticipantLink { get; set; }
    }

    public class RetreatListParticipantViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BedCode { get; set; }
        public DateTime DateReceived { get; set; }
        public string Notes { get; set; }
        public Uri DeleteLink { get; set; }
    }
}
