using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Globalization;
using System.Text.RegularExpressions;
using Dahlia.Services;
using FluentMigrator;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Announcers;
using FluentMigrator.Runner.Generators;
using FluentMigrator.Runner.Initialization;
using FluentMigrator.Runner.Processors;
using FluentMigrator.Runner.Processors.Sqlite;
using FluentMigrator.Runner.Processors.SqlServer;

namespace Dahlia.Configuration.Persistence.Migrations
{
    public class SqlMigrationService : IMigrationService
    {
        MigrationRunner getRunner()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["dahliaSQL"].ConnectionString;
            
            var processor = GetProcessor(connectionString);

            var assembly = typeof(SqlMigrationService).Assembly;
            var ns = typeof(SqlMigrationService).Namespace;

            return new MigrationRunner(assembly, new RunnerContext(new NullAnnouncer()) { Namespace = ns },
                                            processor);
        }

        IMigrationProcessor GetProcessor(string connectionString)
        {
            if (SQLSessionFactory.IsSqliteConnectionString(connectionString))
            {
                var connection = new SQLiteConnection(connectionString);
                var generator = new SqliteGenerator();
                return new SqliteProcessor(connection, generator, new NullAnnouncer(), new ProcessorOptions());
            }
            else
            {
                var connection = new SqlConnection(connectionString);
                var generator = new SqlServer2008Generator();
                return new SqlServerProcessor(connection, generator, new NullAnnouncer(), new ProcessorOptions());
            }
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