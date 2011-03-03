using System;
using System.Collections.Generic;

namespace Dahlia.Services
{
    public interface IMigrationService
    {
        void MigrateUp(long? version);
        void MigrateDown(long version);
    }
}