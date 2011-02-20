using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dahlia.Models;
using NHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Tool.hbm2ddl;

namespace Dahlia.Persistence
{
    public static class SQLSessionFactory
    {
        public static ISessionFactory CreateSessionFactory()
        {
            return Fluently.Configure()
                .Database(
                MsSqlConfiguration.MsSql2005.ConnectionString(
                  c => c.FromConnectionStringWithKey("dahliaSQL")))
                  .Mappings(x => x.FluentMappings
                                 .AddFromAssemblyOf<Retreat>())
                                 //.ExposeConfiguration(BuildSchema)
                                 //.ExposeConfiguration(x => x.SetProperty("current_session_context_class", "web"))
                                 .BuildSessionFactory();
        }

        public static void BuildSchema(NHibernate.Cfg.Configuration cfg)
        {
            new SchemaExport(cfg)
                .Create(false, true);
        }
    }
}
