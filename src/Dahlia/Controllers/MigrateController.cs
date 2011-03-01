using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Dahlia.Configuration.Persistence.Migrations;
using Dahlia.Services;
using Dahlia.ViewModels;

namespace Dahlia.Controllers
{
    public class MigrateController : Controller
    {
        readonly IMigrationService _migrator;
        readonly IMigrationInformation _migrationInformation;

        public MigrateController(IMigrationService migrator, IMigrationInformation migrationInformation)
        {
            _migrator = migrator;
            _migrationInformation = migrationInformation;
        }

        public ViewResult Index()
        {
            var migrationsViewModel = new MigrationsViewModel
            {
                CurrentVersion = _migrationInformation.CurrentVersion().Version,
                AvailableVersions = _migrationInformation.GetMigrations()
                                            .OrderBy(x => x.Item1)
                                            .Select(x => new MigrationViewModel
                                            {
                                                Version = x.Item1, 
                                                Name = x.Item2
                                            })
            };
            return View(migrationsViewModel);
        }

        public ActionResult Up(int? id)
        {
            _migrator.MigrateUp(id);

            return new ContentResult{Content = "Migrated!"};
        }

        public ActionResult Down(int id)
        {
            _migrator.MigrateDown(id);

            return new ContentResult { Content = "Downgraded!" };
        }

    }
}
