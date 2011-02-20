using System;
using System.Collections.Generic;
using System.Linq;
using Dahlia.Models;
using NHibernate;
using NHibernate.Criterion;

namespace Dahlia.Repositories
{
    public interface IRetreatRepository
    {
        IEnumerable<Retreat> GetList();
        void Add(Retreat retreat);
        Retreat Get(DateTime retreatDate);
        void Save(Retreat retreat);
    }

    public class RetreatRepository : IRetreatRepository
    {
        readonly ISession _session;

        public RetreatRepository(ISession session)
        {
            _session = session;
        }

        IEnumerable<Retreat> IRetreatRepository.GetList()
        {
            return _session.CreateCriteria(typeof(Retreat))
                .SetFetchMode("RegisteredParticipants", FetchMode.Lazy)
                .List<Retreat>();
        }

        void IRetreatRepository.Add(Retreat retreat)
        {
            _session.Save(retreat);
            _session.Flush();
        }

        public Retreat Get(DateTime retreatDate)
        {
            return _session.CreateCriteria(typeof (Retreat))
                .Add(Restrictions.Eq("StartDate", retreatDate))
                .UniqueResult<Retreat>();
        }

        public void Save(Retreat retreat)
        {
            _session.SaveOrUpdate(retreat);
            _session.Flush();
        }
    }
}
