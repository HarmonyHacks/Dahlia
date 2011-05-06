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

        public Retreat(IList<Registration> registrations)
        {
            _registrations = registrations;
        }

        public virtual int Id { get; set; }

        public virtual String Description { get; set; }
        
        public virtual DateTime StartDate { get; set; }
        
        public virtual IEnumerable<Registration> Registrations
        {
            get { return _registrations; }
        }

        public virtual bool IsFull
        {
            get
            {
                // TODO: need to check the actual number of existing beds here rather than a magic number
                return Registrations.Where(x => x.Bed != null).Count() >= 29;
            }
        }

        public virtual IEnumerable<Bed> AssignedBeds
        {
            get { return _registrations.Select(r => r.Bed); }
        }

        public virtual IEnumerable<Bed> GetUnassignedBeds(IEnumerable<Bed> beds)
        {
            var comparer = new LambdaComparer<Bed>((first, second) => first.Code == second.Code);
            return beds.Except(AssignedBeds, comparer);
        }

        public virtual void AddParticipant(Participant participant, Bed bed)
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
            var registrationsToRemove = _registrations.Where(r => r.Participant.Id == participantId).ToList();
            foreach (var registration in registrationsToRemove)
            {
                _registrations.Remove(registration);
                registration.Retreat = null;
            }
        }
    }
}
