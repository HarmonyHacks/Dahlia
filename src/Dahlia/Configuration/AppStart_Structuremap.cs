using System.IO;
using System.Web.Mvc;
using Dahlia.Configuration;

[assembly: WebActivator.PreApplicationStartMethod(typeof (AppStart_StructureMap), "Start")]

namespace Dahlia.Configuration
{
    public static class AppStart_StructureMap
    {
        public static void Start()
        {
            var container = IoC.Initialize();
            DependencyResolver.SetResolver(new SmDependencyResolver(container));
        }
    }
}