using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Cfg;
using FluentNHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using System.Reflection;
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
                                 .AddFromAssembly(Assembly.GetAssembly(typeof (Dahlia.Models.Retreat))))
                                 //.ExposeConfiguration(BuildSchema)
                                 .BuildSessionFactory();
        }

        public static void BuildSchema(NHibernate.Cfg.Configuration cfg)
        {
            new SchemaExport(cfg)
                .Create(false, true);
        }
    }
}
