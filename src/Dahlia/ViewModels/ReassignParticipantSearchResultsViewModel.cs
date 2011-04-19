using System.Collections.Generic;

namespace Dahlia.ViewModels
{
    public class ReassignParticipantSearchResultsViewModel
    {
      public string ParticipantName;
        public IEnumerable<ParticipantSearchResultViewModel> Results { get; set; }
    }
}