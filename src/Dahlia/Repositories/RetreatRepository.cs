using System;
using System.Collections.Generic;
using System.Linq;
using Dahlia.Models;
using NHibernate;
using NHibernate.Linq;

namespace Dahlia.Repositories
{
    public interface IRetreatRepository
    {
        IEnumerable<Retreat> GetList();
        void Add(Retreat retreat);
        Retreat Get(DateTime retreatDate);
        Retreat GetById(int id);
        void Save(Retreat retreat);
        void DeleteById(int retreatId);
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
            return _session.Query<Retreat>();
        }

        void IRetreatRepository.Add(Retreat retreat)
        {
            _session.Save(retreat);
            _session.Flush();
        }

        public Retreat Get(DateTime retreatDate)
        {
            return _session.Query<Retreat>()
                .Where(x => x.StartDate == retreatDate)
                .FirstOrDefault();
        }

        public Retreat GetById(int id)
        {
            return _session.Get<Retreat>(id);
        }

        public void Save(Retreat retreat)
        {
            _session.SaveOrUpdate(retreat);
            _session.Flush();
        }

        public void DeleteById(int retreatId)
        {
           var retreat = GetById(retreatId);
            _session.Delete(retreat);
            _session.Flush();
        }
    }
}
