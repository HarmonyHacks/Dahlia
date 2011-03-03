using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dahlia.Models;
using NHibernate;
using NHibernate.Linq;

namespace Dahlia.Repositories
{
    public interface IVersionRepository
    {
        VersionInfo GetCurrent();
    }

    public class VersionRepository : IVersionRepository
    {
        readonly ISession _session;

        public VersionRepository(ISession session)
        {
            _session = session;
        }

        public VersionInfo GetCurrent()
        {
            var version = new VersionInfo();
            try
            {
                version = _session.Query<VersionInfo>()
                    .ToList()
                    .LastOrDefault();
            }
            catch(ADOException e)
            {
                //this is an ugly hack to get around non existant VersionInfo table
                if (!e.Message.Contains("VersionInfo"))
                    throw;
            }
            return version;

        }
    }
}