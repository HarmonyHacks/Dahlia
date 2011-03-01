using System;
using System.Collections.Generic;
using System.Linq;
using Dahlia.Models;
using Dahlia.Repositories;
using FluentMigrator;
using NHibernate;

namespace Dahlia.Configuration.Persistence.Migrations
{
    public interface IMigrationInformation
    {
        VersionInfo CurrentVersion();
        IEnumerable<Tuple<long, string>> GetMigrations();
    }

    public class MigrationInformation : IMigrationInformation
    {
        readonly IVersionRepository _versionRepository;

        public MigrationInformation(IVersionRepository versionRepository)
        {
            _versionRepository = versionRepository;
        }

        public VersionInfo CurrentVersion()
        {
            return _versionRepository.GetCurrent();
        }

        public IEnumerable<Tuple<long, string>> GetMigrations()
        {
           return typeof(SqlMigrationService).Assembly.GetTypes()
                .Where(x => x.BaseType == typeof(Migration))
                .Select(MakeTuple);
        }

        Tuple<long,string> MakeTuple(Type type)
        {
            var name = type.Name;
            var version = type.GetCustomAttributes(typeof (MigrationAttribute), false)
                              .Cast<MigrationAttribute>()
                              .First().Version;

            return Tuple.Create(version, name);
        }
    }
}