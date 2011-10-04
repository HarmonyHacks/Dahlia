using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Dahlia.Configuration;

[assembly: WebActivator.PreApplicationStartMethod(typeof (AppStart_StructureMap), "Start")]

namespace Dahlia.Configuration
{
    public static class AppStart_StructureMap
    {
        public static void Start()
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(HttpContext.Current.Server.MapPath("~/log4net.config")));
            var container = IoC.Initialize();
            DependencyResolver.SetResolver(new SmDependencyResolver(container));
        }
    }
}