using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dahlia.Models;
using FluentNHibernate.Automapping;
using NHibernate;
using NHibernate.Cfg;
using FluentNHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using System.Reflection;
using NHibernate.Tool.hbm2ddl;

namespace Dahlia.Specifications
{
    public static class SQLiteSessionFactory
    {
        public static ISessionFactory CreateSessionFactory()
        {
            return Fluently.Configure()
                .Database(
                SQLiteConfiguration.Standard.ConnectionString(
                  c => c.FromConnectionStringWithKey("dahlia")))
                  .Mappings(x => x.AutoMappings.Add(
                                    AutoMap.AssemblyOf<IAmPersistable>(
                                        type => type.GetInterfaces()
                                            .Any(z => z == typeof(IAmPersistable))
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
            new SchemaExport(cfg)
                .Create(false, true);
        }
    }
}
