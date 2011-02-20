using System.Collections.Generic;
using Dahlia.Models;

namespace Dahlia.Repositories
{
    public interface ParticipantRepository
    {
        IEnumerable<Participant> WithLastName(string Lastname);
        void Add(IEnumerable<Participant> expectedMatches);
    }
}