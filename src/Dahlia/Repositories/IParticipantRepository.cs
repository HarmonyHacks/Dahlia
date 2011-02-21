using System.Collections.Generic;
using Dahlia.Models;

namespace Dahlia.Repositories
{
    public interface IParticipantRepository
    {
        IEnumerable<Participant> WithLastName(string lastName);
        void Add(IEnumerable<Participant> expectedMatches);
        Participant GetById(int id);
        IEnumerable<Participant> WithNameLike(string firstName, string lastName);
    }
}