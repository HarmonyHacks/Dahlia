using System;
using System.Collections.Generic;

namespace Dahlia.ViewModels
{
    public class ReassignParticipantSearchResultsViewModel
    {
        public IEnumerable<ReassignParticipantSearchResultViewModel> Results { get; set; }
    }

    public class ReassignParticipantSearchResultViewModel
    {
        public string Name { get; set; }
        public DateTime DateReceived { get; set; }
        public Uri SelectLink { get; set; } 
    }
}