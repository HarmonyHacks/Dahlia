using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using Dahlia.Persistence;
using log4net;
using NHibernate;
using StructureMap;

namespace Dahlia.Configuration
{
    public static class IoC
    {
        public static IContainer Initialize()
        {
            return Initialize(true);
        }

        public static IContainer Initialize(bool useRealDatabaseObjects)
        {
            ObjectFactory.Initialize(x =>
            {
                x.Scan(scan =>
                {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                });

                if (useRealDatabaseObjects)
                {
                    x.For<ISessionFactory>().Singleton().Use(SQLSessionFactory.CreateSessionFactory());
                    x.For<ISession>().HybridHttpOrThreadLocalScoped().Use(context => context.GetInstance<ISessionFactory>().OpenSession());
                }
                else
                {
                    x.For<ISession>().HybridHttpOrThreadLocalScoped().Use(() => null);
                }

                x.For<ILog>().Use(LogManager.GetLogger("Dahlia"));
                x.For<RouteCollection>().Use(RouteTable.Routes);
                x.FillAllPropertiesOfType<ILog>().Use(LogManager.GetLogger("Dahlia"));
            });

            return ObjectFactory.Container;
        }
    }
}