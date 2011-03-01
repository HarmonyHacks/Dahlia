using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Dahlia.Models;
using FluentNHibernate.Automapping;
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
            // If you get a "failed to attach" exception here, that's because you need to create a database file.
            // In VS, right-click the Dahlia\App_Data folder, choose Add | New Item | SQL Server Database, name it Dahlia.mdf.
            return Fluently.Configure()
                .Database(
                    MsSqlConfiguration.MsSql2005.ConnectionString(
                        c => c.FromConnectionStringWithKey("dahliaSQL")))
                .Mappings(x => x.AutoMappings.Add(
                    AutoMap.AssemblyOf<IAmPersistable>(
                        type => type.GetInterfaces()
                            .Any(z => z == typeof (IAmPersistable))
                        )
                        .UseOverridesFromAssemblyOf<IAmPersistable>()
                        .Conventions.AddFromAssemblyOf<IAmPersistable>()
                     )
                )
                .ExposeConfiguration(BuildSchema)
                .BuildSessionFactory();
        }

        public static void BuildSchema(NHibernate.Cfg.Configuration cfg)
        {
            bool rebuildSchema = bool.Parse(ConfigurationManager.AppSettings["rebuildSchema"]);
            bool updateSchema = bool.Parse(ConfigurationManager.AppSettings["updateSchema"]);

            if (rebuildSchema)
            {
                new SchemaExport(cfg).Create(false, true);
            }
            else if (updateSchema)
            {
                new SchemaUpdate(cfg).Execute(false, true);
            }
        }
    }
}