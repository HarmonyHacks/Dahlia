using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dahlia.Models
{
    public class CareGiver
    {
        public virtual int Id { get; set; }
        public virtual IList<Participant> Participants { get; set; }
    }
}
