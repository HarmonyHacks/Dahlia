using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dahlia.Models
{
    public class Participant : IAmPersistable
    {
        public virtual int Id { get; set; } 
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual DateTime DateReceived { get; set; }
        public virtual string Notes { get; set; }
        public virtual PhysicalStatus PhysicalStatus { get; set; }

        // override object.Equals
        public override bool Equals(object obj)
        {
            //       
            // See the full list of guidelines at
            //   http://go.microsoft.com/fwlink/?LinkID=85237  
            // and also the guidance for operator== at
            //   http://go.microsoft.com/fwlink/?LinkId=85238
            //

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = obj as Participant;

            return Id == other.Id;
        }

// override object.GetHashCode
        public override int GetHashCode()
        {
            // TODO: write your implementation of GetHashCode() here
            throw new NotImplementedException();
            return base.GetHashCode();
        }
    }
}
