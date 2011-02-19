using System;

namespace Dahlia.ViewModels
{
    public class AddParticipantToRetreatViewModel
    {
        public DateTime RetreatDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BedCode { get; set; }
        public DateTime DateReceived { get; set; }
        public string Notes { get; set; }
    }
}