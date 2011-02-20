using System;
using System.Collections.Generic;
using System.Linq;
using Dahlia.Models;
using NHibernate;

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
        static ICollection<Retreat> _Retreats;
        static ISession _session;

        static RetreatRepository()
        {
            _Retreats = new List<Retreat>
            {
                new Retreat { StartDate = new DateTime(2011, 4, 1)},
                new Retreat { StartDate = new DateTime(2011, 6, 1)},
                new Retreat { StartDate = new DateTime(2011, 7, 15)},
            };
        }

        public RetreatRepository(ISession session)
        {
            _session = session;
        }

        IEnumerable<Retreat> IRetreatRepository.GetList()
        {
            return _Retreats;
        }

        void IRetreatRepository.Add(Retreat retreat)
        {
            _Retreats.Add(retreat);
        }

        public Retreat Get(DateTime retreatDate)
        {
            return _Retreats.First(r => r.StartDate == retreatDate);
        }

        public void Save(Retreat retreat)
        {
            _session.SaveOrUpdate(retreat);
        }
    }
}
