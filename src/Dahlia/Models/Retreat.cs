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
            Registrations = new List<Participant>();
        }

        public virtual int Id { get; set; }
        public virtual IList<Participant> Registrations { get; set; }
        public virtual String Description { get; set; }
        public virtual DateTime StartDate { get; set; }

        public virtual bool IsFull
        {
            get
            {
                return Registrations.Where(x => !string.IsNullOrEmpty(x.BedCode)).Count() >= 29;
            }
        }



        public virtual void AddParticipant(Participant NewRegistration )
        {
            Registrations.Add(NewRegistration);
        }
    }

}
