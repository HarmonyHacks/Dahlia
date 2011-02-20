using System.Collections.Generic;
using Dahlia.Models;
using NHibernate;

namespace Dahlia.Repositories
{
    public class ParticipantRepositoryNHibImpl 
        : ParticipantRepository
    {
        ISession CurrentSession;

        public ParticipantRepositoryNHibImpl(ISession currentSession)
        {
            this.CurrentSession = currentSession;
        }

        IEnumerable<Participant> ParticipantRepository.WithLastName(string Lastname)
        {
            const string Query = @"from Participant p where p.LastName = :LastName";

            var Partcipants = CurrentSession.CreateQuery(Query);
            Partcipants.SetParameter("LastName", Lastname);

            return  Partcipants.List<Participant>();
        }

        void ParticipantRepository.Add(IEnumerable<Participant> Participants)
        {
            foreach (var P in Participants)
            {
                CurrentSession.SaveOrUpdate(P);
            }
        }
    }
}