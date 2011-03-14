using System;
using System.Collections.Generic;
using System.Linq;
using Dahlia.Models;
using NHibernate;
using NHibernate.Linq;

namespace Dahlia.Repositories
{
    public class ParticipantRepository : IParticipantRepository
    {
        readonly ISession _session;

        public ParticipantRepository(ISession session)
        {
            _session = session;
        }

        IEnumerable<Participant> IParticipantRepository.WithLastName(string lastName)
        {
            return _session.Query<Participant>()
                .Where(x => x.LastName.Contains(lastName));
        }

        void IParticipantRepository.Add(IEnumerable<Participant> participants)
        {
            foreach (var p in participants)
                _session.SaveOrUpdate(p);
            _session.Flush();
        }

        Participant IParticipantRepository.GetById(int id)
        {
            return _session.Get<Participant>(id);
        }

        public IEnumerable<Participant> WithNameLike(string firstName, string lastName)
        {
            if (lastName == null)
                lastName = string.Empty;
            if (firstName == null)
                firstName = string.Empty;
            
            return _session.Query<Participant>()
                .Where(x => x.FirstName == firstName)
                .Where(x => x.LastName == lastName);
        }
    }
}