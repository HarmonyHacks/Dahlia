using System;
using System.Collections.Generic;
using Dahlia.Configuration.Persistence.Migrations;
using Machine.Specifications;

namespace Dahlia.Specifications.Configuration.Persistence.Migrations
{
    public class When_getting_the_available_versions
    {
        Establish context = () =>
        { _item = new MigrationInformation(null); };

        Because of = () =>
            _result = _item.GetMigrations();

        It should_get_the_migration_data = () =>
            _result.ShouldNotBeEmpty();

        static MigrationInformation _item;
        static IEnumerable<Tuple<long, string>> _result;
    }
}