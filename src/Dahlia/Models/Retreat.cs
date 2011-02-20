using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dahlia.Models
{
    public class Retreat
    {
        public Retreat()
        {
            RegisteredParticipants = new List<RegisteredParticipant>();
        }

        public virtual int Id { get; set; }
        //public virtual IList<Room> Rooms { get; set; }
        //public virtual IList<WaitListing> WaitList { get; set; }
        //public virtual IList<CareGiver> CareGivers { get; set; }
        
        public virtual IList<RegisteredParticipant> RegisteredParticipants { get; set; }
        public virtual String Description { get; set; }
        public virtual DateTime StartDate { get; set; }
        public virtual String StartDateStr { get { return StartDate.ToShortDateString(); } }
        
        //public virtual DateTime EndDate { get; set; }

        public virtual void AddParticipant(Participant newParticipant, string bedCode, PhysicalStatus physicalStatus)
        {
            var newRegisteredParticipant = new RegisteredParticipant
            {
                Participant = newParticipant,
                Retreat = this,
                BedCode = bedCode,
                PhysicalStatus = physicalStatus,
            };

            RegisteredParticipants.Add(newRegisteredParticipant);
        }

    }

    public class RegisteredParticipant
    {
        public virtual Participant Participant { get; set;  }
        public virtual string BedCode { get; set; }
        public virtual Retreat Retreat { get; set; }
        public virtual PhysicalStatus PhysicalStatus { get; set; }
    }
}
