using System.Collections.Generic;
using Dahlia.Models;
using NHibernate;
using NHibernate.Criterion;

namespace Dahlia.Repositories
{
    public interface IBedRepository
    {
        IEnumerable<Bed> GetAll();
        Bed GetBy(string code);
    }

    public class BedRepository : IBedRepository
    {
        readonly ISession _session;

        public BedRepository(ISession session)
        {
            _session = session;
        }

        public IEnumerable<Bed> GetAll()
        {
            return _session.CreateCriteria(typeof (Bed))
                .List<Bed>();
        }

        public Bed GetBy(string code)
        {
            return _session.CreateCriteria(typeof (Bed))
                .Add(Restrictions.Eq("Code", code))
                .UniqueResult<Bed>();
        }
    }
}