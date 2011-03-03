using System;
using Dahlia.Services;

namespace Dahlia.ViewModels
{
    public class DeleteParticipantFromRetreatViewModel
    {
        public int RetreatId { get; set; }

        public DateTime RetreatDate { get; set; }
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
    }
}