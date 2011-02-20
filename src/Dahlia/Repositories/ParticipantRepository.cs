using System;
using System.Collections.Generic;
using Dahlia.Models;
using NHibernate;

namespace Dahlia.Repositories
{
    public class ParticipantRepository : IParticipantRepository
    {
        readonly ISession _currentSession;

        public ParticipantRepository(ISession currentSession)
        {
            _currentSession = currentSession;
        }

        IEnumerable<Participant> IParticipantRepository.WithLastName(string lastName)
        {
            const string query = @"from Participant p where p.LastName = :LastName";

            var participants = _currentSession.CreateQuery(query);
            participants.SetParameter("LastName", lastName);

            return participants.List<Participant>();
        }

        void IParticipantRepository.Add(IEnumerable<Participant> participants)
        {
            foreach (var p in participants)
                _currentSession.SaveOrUpdate(p);
            _currentSession.Flush();
        }

        Participant IParticipantRepository.GetById(int id)
        {
            return _currentSession.Get<Participant>(id);
        }
    }
}