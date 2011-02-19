using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dahlia.Models
{
    public class Participant
    {
        public virtual int Id { get; set; } 
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual DateTime DateReceived { get; set; }
        public virtual string Notes { get; set; }
    }
}
