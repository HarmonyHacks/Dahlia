using System;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Dahlia.Models;
using FluentNHibernate.Automapping;
using NHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;

namespace Dahlia.Configuration.Persistence
{
    public static class SQLSessionFactory
    {
        public static ISessionFactory CreateSessionFactory()
        {
            var configurator = CreatePersistanceConfigurator();

            return Fluently.Configure()
                .Database(configurator)
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

        static IPersistenceConfigurer CreatePersistanceConfigurator()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["dahliaSQL"].ConnectionString;

            if (IsSqliteConnectionString(connectionString))
            {
                return SQLiteConfiguration.Standard.ConnectionString(
                    c => c.FromConnectionStringWithKey("dahliaSQL"));
            }
            else
            {
                return MsSqlConfiguration.MsSql2008.ConnectionString(
                    c => c.FromConnectionStringWithKey("dahliaSQL"));
            }
        }

        public static bool IsSqliteConnectionString(string connectionString)
        {
            return Regex.IsMatch(connectionString, @"Data Source=.*\.db(;|\z)", RegexOptions.IgnoreCase);
        }
    }
}
