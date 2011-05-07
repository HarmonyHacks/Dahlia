using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Dahlia.Configuration.Persistence.Migrations;
using Dahlia.Services;
using HibernatingRhinos.Profiler.Appender.NHibernate;
using StructureMap;

namespace Dahlia
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Retreat", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        protected void Application_Start()
        {
            NHibernateProfiler.Initialize();
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            bool autoMigrate;
            if (bool.TryParse(ConfigurationManager.AppSettings["AutoMigrateDatabase"], out autoMigrate))
                AutoMigrateDatabase(autoMigrate);
        }

        private void AutoMigrateDatabase(bool autoMigrate)
        {
            if (!autoMigrate) return;

            var migrationInfo = ObjectFactory.GetInstance<IMigrationInformation>();
            var currentVersion = migrationInfo.CurrentVersion().Version;
            var hasVersionsToUpdate = migrationInfo.GetMigrations()
                .Any(m => m.Item1 > currentVersion);

            if (hasVersionsToUpdate)
            {
                var migrationService = ObjectFactory.GetInstance<IMigrationService>();
                migrationService.MigrateUp(null); //should migrate up all versions
            }
        }

        protected void Application_EndRequest()
        {
            ObjectFactory.ReleaseAndDisposeAllHttpScopedObjects();
        } 
    }
}