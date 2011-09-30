using System;
using System.Collections.Generic;
using Dahlia.Models;

namespace Dahlia.ViewModels
{
    public class EditParticipantViewModel
    {
        public EditParticipantViewModel()
        {
            CurrentRegistrations = new List<CurrentRegistrationViewModel>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateReceived { get; set; }
        public string Notes { get; set; }
        public PhysicalStatus PhysicalStatus { get; set; }
        public List<CurrentRegistrationViewModel> CurrentRegistrations { get; set; }
    }

    public class CurrentRegistrationViewModel
    {
        public int Id { get; set; }
        public string RetreatName { get; set; }
        public string BedCode { get; set; }
        public string[] AvailableBedCodes { get; set;} 
    }
}