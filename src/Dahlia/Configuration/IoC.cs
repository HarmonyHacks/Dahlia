using log4net;
using StructureMap;
using StructureMap.Configuration.DSL;
using StructureMap.TypeRules;

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
                            x.FillAllPropertiesOfType<ILog>().Use(LogManager.GetLogger("Dahlia"));

                        });
            return ObjectFactory.Container;
        }
    }
}