using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dahlia.Models
{
    public class Retreat : IAmPersistable
    {
        IList<Registration> _registrations;

        public Retreat()
        {
            _registrations = new List<Registration>();
        }

        public virtual int Id { get; set; }

        public virtual String Description { get; set; }
        
        public virtual DateTime StartDate { get; set; }
        
        public virtual IEnumerable<Registration> Registrations
        {
            get { return _registrations; }
            set { _registrations = value as IList<Registration>; }
        }

        public virtual bool IsFull
        {
            get
            {
                // TODO: need to check the actual number of existing beds here rather than a magic number
                return Registrations.Where(x => x.Bed != null).Count() >= 29;
            }
        }

        public virtual void AddParticipant(Participant participant, Bed bed )
        {
            EnsureRetreatIsNotFull();
            EnsureBedIsNotAssigned(bed);

            var registration = new Registration {Participant = participant, Retreat = this, Bed = bed};
            _registrations.Add(registration);
        }

        void EnsureRetreatIsNotFull()
        {
            if (IsFull)
            {
                throw new InvalidOperationException("Cannot add a participant to a full retreat.");
            }
        }

        void EnsureBedIsNotAssigned(Bed bed)
        {
            if (Registrations.Any(r => r.Bed == bed))
            {
                throw new InvalidOperationException("Cannot assign the same bed twice for the same retreat.");
            }
        }

        public virtual void RemoveParticipant(int participantId)
        {
            ((List<Registration>)Registrations).RemoveAll(r => r.Participant.Id == participantId);
        }
    }
}
