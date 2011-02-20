using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dahlia.Models
{
    public class Retreat : IAmPersistable
    {
        public Retreat()
        {
            Registrations = new List<Registration>();
        }

        public virtual int Id { get; set; }
        public virtual IList<Registration> Registrations { get; set; }
        public virtual String Description { get; set; }
        public virtual DateTime StartDate { get; set; }

        public virtual bool IsFull
        {
            get
            {
                return Registrations.Where(x => !string.IsNullOrEmpty(x.BedCode)).Count() >= 29;
            }
        }

        public virtual void AddParticipant(Participant newRegistration, string bedCode )
        {
            var registration = new Registration {Participant = newRegistration, Retreat = this, BedCode = bedCode};
            Registrations.Add(registration);
        }
    }
}
