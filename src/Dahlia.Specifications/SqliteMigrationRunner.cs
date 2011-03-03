using System;
using System.Data.SQLite;
using System.Diagnostics;
using Dahlia.Configuration.Persistence.Migrations;
using Dahlia.Services;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Announcers;
using FluentMigrator.Runner.Generators;
using FluentMigrator.Runner.Initialization;
using FluentMigrator.Runner.Processors;
using FluentMigrator.Runner.Processors.Sqlite;

namespace Dahlia.Specifications
{
    public class SqliteMigrationRunner : IMigrationService
    {
       MigrationRunner _runner;

       public SqliteMigrationRunner(SQLiteConnection connection)
        {
            var assembly = typeof(SqlMigrationService).Assembly;
            var ns = typeof(SqlMigrationService).Namespace;
            var generator = new SqliteGenerator();
            var processor = new SqliteProcessor(connection, generator, new TextWriterAnnouncer(Console.Out), new ProcessorOptions());
            
            _runner = new MigrationRunner(assembly, new RunnerContext(new TextWriterAnnouncer(Console.Out)) { Namespace = ns },
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