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
        public virtual IList<RegisteredParticipant> RegisteredParticipants { get; set; }
        public virtual String Description { get; set; }
        public virtual DateTime StartDate { get; set; }
        public virtual String StartDateStr { get { return StartDate.ToShortDateString(); } }
        
    }

    public class RegisteredParticipant
    {
        public virtual Participant Participant { get; set;  }
        public virtual string BedCode { get; set; }
        public virtual Retreat Retreat { get; set; }
    }
}
