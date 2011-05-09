using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dahlia.Models;

namespace Dahlia.ViewModels
{
    public class EditParticipantViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateReceived { get; set; }
        public string Notes { get; set; }
        public PhysicalStatus PhysicalStatus { get; set; }
    }
}