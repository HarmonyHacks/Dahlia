namespace Dahlia.Services
{
    public interface IMigrationService
    {
        void MigrateUp(long? version);
        void MigrateDown(long version);
    }
}