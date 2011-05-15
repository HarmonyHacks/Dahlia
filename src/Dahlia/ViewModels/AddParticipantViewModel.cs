using System;
using System.Collections.Generic;
using Dahlia.Models;

namespace Dahlia.ViewModels
{
    public class AddParticipantViewModel
    {
        public int RetreatId { get; set; }
        public string BedCode { get; set; }
        public bool RetreatIsFull { get; set; }
        public string Cancel { get; set; }
        public string Save { get; set; }
        public string Search { get; set; }
        public IEnumerable<Bed> Beds { get; set; }

        public CreateParticipantViewModel Participant { get; set; }

        public List<ParticipantSearchResultViewModel> SearchResults;
    }
}