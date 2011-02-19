using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dahlia.Models
{
    public class Retreat
    {
        public virtual int Id { get; set; }
        public virtual IList<Room> Rooms { get; set; }
        public virtual IList<WaitListing> WaitList { get; set; }
        public virtual IList<Participant> Participants { get; set; }
        public virtual IList<CareGiver> CareGivers { get; set; }
        public virtual String Description { get; set; }
        public virtual DateTime StartDate { get; set; }
        public virtual DateTime EndDate { get; set; }
    }
}
