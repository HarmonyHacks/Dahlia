using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Dahlia.Services;

namespace Dahlia.Controllers
{
    public class MigrateController : Controller
    {
        readonly IMigrationService _migrator;

        public MigrateController(IMigrationService migrator)
        {
            _migrator = migrator;
        }

        public ActionResult Index()
        {
            return View();
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
