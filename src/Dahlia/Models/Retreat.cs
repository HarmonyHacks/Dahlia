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

        public virtual void AddParticipant(Participant participant, Bed bed )
        {
            var registration = new Registration {Participant = participant, Retreat = this, Bed = bed};
            Registrations.Add(registration);
        }

        public virtual void RemoveParticipant(int participantId)
        {
            ((List<Registration>)Registrations).RemoveAll(r => r.Participant.Id == participantId);
        }
    }
}
