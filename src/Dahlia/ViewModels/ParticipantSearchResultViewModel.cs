using System;

namespace Dahlia.ViewModels
{
    public class ParticipantSearchResultViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateReceived { get; set; }
        public Uri SelectLink { get; set; }
    }
}