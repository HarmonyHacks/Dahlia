using System;
using System.Collections.Generic;
using Dahlia.Models;

namespace Dahlia.Services
{
    public interface IMigrationService
    {
        void MigrateUp(long? version);
        void MigrateDown(long version);
    }
}