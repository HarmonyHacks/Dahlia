using System;
using System.Collections.Generic;
using Dahlia.Models;
using Dahlia.Services;

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
        public PhysicalStatus PhysicalStatus { get; set; }
        public bool RetreatIsFull { get; set; }
        public string Cancel { get; set; }
        public string Save { get; set; }
        public string Search { get; set; }

        public string RetreatUiId
        {
            get { return RetreatUiHelpers.RetreatUiId(RetreatDate); }
        }
    }
}