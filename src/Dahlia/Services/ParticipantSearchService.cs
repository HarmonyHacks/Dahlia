using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dahlia.Models;

namespace Dahlia.Services
{
    public interface IParticipantSearchService
    {
        IEnumerable<Participant> SearchParticipants(string firstName, string lastName);
    }

    public class ParticipantSearchService: IParticipantSearchService
    {
        public IEnumerable<Participant> SearchParticipants(string firstName, string lastName)
        {
            return new[]
            {
                new Participant {FirstName = "bob", LastName = "fred", DateReceived = DateTime.Now},
                new Participant {FirstName = "allen", LastName = "wikipedia", DateReceived = DateTime.Now},
            };
        }
    }
}