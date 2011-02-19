using System.Web.Mvc;
using Dahlia.Configuration;
using StructureMap;

[assembly: WebActivator.PreApplicationStartMethod(typeof(AppStart_Structuremap), "Start")]

namespace Dahlia.Configuration {
    public static class AppStart_Structuremap {
        public static void Start() {
            var container = IoC.Initialize();
            //DependencyResolver.SetResolver(new SmDependencyResolver(container));
            ControllerBuilder.Current.SetControllerFactory(new SmControllerFactory(container));
        }
    }
}