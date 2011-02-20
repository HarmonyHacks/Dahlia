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
                return Registrations.Where(x => x.Bed != null).Count() >= 29;
            }
        }

        public virtual void AddParticipant(Participant newRegistration, Bed bed )
        {
            var registration = new Registration {Participant = newRegistration, Retreat = this, Bed = bed};
            Registrations.Add(registration);
        }
    }
}
