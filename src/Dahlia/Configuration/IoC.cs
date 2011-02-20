
﻿using System.Web.Routing;
using System.Web.Script.Serialization;
using log4net;
using StructureMap;

namespace Dahlia.Configuration {
    public static class IoC {
        public static IContainer Initialize() {
            ObjectFactory.Initialize(x =>
                        {
                            x.Scan(scan =>
                                    {
                                        scan.TheCallingAssembly();
                                        scan.WithDefaultConventions();
                                    });
                            
                            x.For<ILog>().Use(LogManager.GetLogger("Dahlia"));
                            x.For<RouteCollection>().Use(RouteTable.Routes);
                            x.Register(new JavaScriptSerializer());
                            x.FillAllPropertiesOfType<ILog>().Use(LogManager.GetLogger("Dahlia"));

                        });
            return ObjectFactory.Container;
        }
    }
}