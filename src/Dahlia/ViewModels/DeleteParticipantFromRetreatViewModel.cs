using System;
using Dahlia.Services;

namespace Dahlia.ViewModels
{
    public class DeleteParticipantFromRetreatViewModel
    {
        public DateTime RetreatDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string RetreatUiId
        {
            get { return RetreatUiHelpers.RetreatUiId(RetreatDate); }
        }
    }
}