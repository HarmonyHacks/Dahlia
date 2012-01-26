using System.Collections.Generic;
using System.Linq;
using Dahlia.Models;
using NHibernate;
using NHibernate.Linq;

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
            return _session.Query<Bed>().OrderBy(b => b.Code);
        }

        public Bed GetBy(string code)
        {
            return _session.Query<Bed>()
                .Where(x => x.Code == code)
                .OrderBy(b => b.Code)
                .FirstOrDefault();
        }
    }
}