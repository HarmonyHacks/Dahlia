using System;
using System.Data.SQLite;
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
        readonly SQLiteConnection _connection;

       public SqliteMigrationRunner(SQLiteConnection connection)
        {
           _connection = connection;
        }

        MigrationRunner getRunner()
        {
            var assembly = typeof(SqlMigrationService).Assembly;
            var ns = typeof(SqlMigrationService).Namespace;
            var generator = new SqliteGenerator();
            var processor = new SqliteProcessor(_connection, generator, new TextWriterAnnouncer(Console.Out), new ProcessorOptions());

            return new MigrationRunner(assembly, new RunnerContext(new TextWriterAnnouncer(Console.Out)) { Namespace = ns },
                                          processor); 
        }

        public void MigrateUp(long? version)
        {
            if(version.HasValue)
               getRunner().MigrateUp(version.Value);
            else
                getRunner().MigrateUp();
        }

        public void MigrateDown(long version)
        {
            getRunner().MigrateDown(version);
        }
    } 
}