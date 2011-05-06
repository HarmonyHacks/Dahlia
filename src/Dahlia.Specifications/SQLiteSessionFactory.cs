using System.Data.SQLite;
using System.Linq;
using Dahlia.Models;
using FluentNHibernate.Automapping;
using NHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;

namespace Dahlia.Specifications
{
    public static class SQLiteSessionFactory
    {
        public static ISessionFactory CreateSessionFactory()
        {
            return Fluently.Configure()
                .Database(
                SQLiteConfiguration.Standard.InMemory())
                  .Mappings(x => x.AutoMappings.Add(
                                    AutoMap.AssemblyOf<IAmPersistable>(
                                        type => type.GetInterfaces()
                                            .Any(z => z == typeof(IAmPersistable))
                                            )
                                           .UseOverridesFromAssemblyOf<IAmPersistable>()
                                           .Conventions.AddFromAssemblyOf<IAmPersistable>()
                                    )

                                )
                                 .BuildSessionFactory();

            
                              
        }
    }
}
