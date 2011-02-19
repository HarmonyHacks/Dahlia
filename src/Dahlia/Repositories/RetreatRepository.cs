using System;
using System.Collections.Generic;
using Dahlia.Models;

namespace Dahlia.Repositories
{
    public interface IRetreatRepository
    {
        IEnumerable<Retreat> GetList();
        void Add(Retreat retreat);
    }

    public class RetreatRepository : IRetreatRepository
    {
        static ICollection<Retreat> _Retreats;

        static RetreatRepository()
        {
            _Retreats = new List<Retreat>();
        }

        public IEnumerable<Retreat> GetList()
        {
            return _Retreats;
        }

        public void Add(Retreat retreat)
        {
            _Retreats.Add(retreat);
        }
    }
}