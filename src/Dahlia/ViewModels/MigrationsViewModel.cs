using System;
using System.Collections.Generic;
using System.Linq;

namespace Dahlia.ViewModels
{
    public class MigrationsViewModel
    {
        public MigrationsViewModel()
        {
            AvailableVersions = Enumerable.Empty<MigrationViewModel>();
        }
        public IEnumerable<MigrationViewModel> AvailableVersions;
        public int CurrentVersion { get; set; }
    }

    public class MigrationViewModel
    {
        public long Version { get; set; }
        public string Name { get; set; }
    }
}