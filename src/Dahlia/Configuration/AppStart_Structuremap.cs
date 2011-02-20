using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Dahlia.Configuration;
using WebActivator;

[assembly: PreApplicationStartMethod(typeof (AppStart_StructureMap), "Start")]

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