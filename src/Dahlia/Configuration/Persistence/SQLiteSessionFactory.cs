using System.Linq;
using Dahlia.Models;
using FluentNHibernate.Automapping;
using NHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Tool.hbm2ddl;

namespace Dahlia.Configuration.Persistence
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
