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
            string resultText;
            var version = _migrationInformation.CurrentVersion().Version;

            if (version == id)
            {
                resultText = "Already on this version.";
            }
            else if (id.HasValue && version > id)
            {
                resultText = "On a newer version.";
            }
            else
            {
                _migrator.MigrateUp(id);
                resultText = "Migrated!";
            }

            return new ContentResult{Content = resultText};
        }

        public ActionResult Down(int id)
        {
            string resultText;
            var version = _migrationInformation.CurrentVersion().Version;

            if ((version + 1) == id)
            {
                resultText = "Already on this version.";
            }
            else if (version < id)
            {
                resultText = "On an older version.";
            }
            else
            {
                _migrator.MigrateDown(id);
                resultText = "Downgraded!";
            }

            return new ContentResult { Content = resultText };
        }

    }
}
