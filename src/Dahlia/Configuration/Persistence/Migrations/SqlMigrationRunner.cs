using System.Configuration;
using System.Data.SqlClient;
using Dahlia.Services;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Announcers;
using FluentMigrator.Runner.Generators;
using FluentMigrator.Runner.Initialization;
using FluentMigrator.Runner.Processors;
using FluentMigrator.Runner.Processors.SqlServer;

namespace Dahlia.Configuration.Persistence.Migrations
{
    public class SqlMigrationService : IMigrationService
    {
        MigrationRunner _runner;

        public SqlMigrationService()
        {
            var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["dahliaSQL"].ConnectionString);
            var assembly = typeof(SqlMigrationService).Assembly;
            var ns = typeof(SqlMigrationService).Namespace;
            var generator = new SqlServer2005Generator();
            var processor = new SqlServerProcessor(connection, generator, new NullAnnouncer(), new ProcessorOptions());
            
            _runner = new MigrationRunner(assembly, new RunnerContext(new NullAnnouncer()) { Namespace = ns },
                                          processor);

        }

        public void MigrateUp(long? version)
        {
            if(version.HasValue)
                _runner.MigrateUp(version.Value);
            else
                _runner.MigrateUp();
        }

        public void MigrateDown(long version)
        {
            _runner.MigrateDown(version);
        }
    }
}