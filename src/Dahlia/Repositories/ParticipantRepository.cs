using System.Collections.Generic;
using Dahlia.Models;

namespace Dahlia.Repositories
{
    public interface IParticipantRepository
    {
        IEnumerable<Participant> GetAll();
    }

    public class ParticipantRepository : IParticipantRepository
    {
        public IEnumerable<Participant> GetAll()
        {
            return new List<Participant>
                   {
                       new Participant {FirstName = "First Name 1", LastName = "Last Name 1"},
                       new Participant {FirstName = "First Name 2", LastName = "Last Name 2"},
                       new Participant {FirstName = "First Name 3", LastName = "Last Name 3"},
                       new Participant {FirstName = "First Name 4", LastName = "Last Name 4"}
                   };
        }
    }
}