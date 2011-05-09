using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Dahlia.Configuration.Persistence.Migrations;
using Dahlia.Controllers;
using Dahlia.Models;
using Dahlia.Services;
using Machine.Specifications;
using Rhino.Mocks;

namespace Dahlia.Specifications.Controllers.Migrations
{
    public class When_migrating_down_to_a_newer_version
    {
        Establish context = () =>
        {
            _version = new VersionInfo {Version = 1};
            _migrations = new[] {Tuple.Create(1L, "foo"), Tuple.Create(2L, "bar")};
            _migrationInformation = MockRepository.GenerateStub<IMigrationInformation>();
            _migrationInformation.Stub(x => x.CurrentVersion()).Return(_version);
            _migrationInformation.Stub(x => x.GetMigrations()).Return(_migrations);
            _migrationService = MockRepository.GenerateMock<IMigrationService>();
            _migrationService.Expect(x => x.MigrateUp(Arg<int>.Is.Anything)).Repeat.Never();
            _controller = new MigrateController(_migrationService, _migrationInformation);
        };

        Because of = () =>
        {
            _firstUpResult = (ContentResult)_controller.Down(3);
        };

        It should_not_migrate = () =>
            _migrationService.VerifyAllExpectations();

        It should_ignore_the_call_with_a_nice_note = () =>
            _firstUpResult.Content.ShouldEqual("On an older version.");

        static IMigrationInformation _migrationInformation;
        static MigrateController _controller;
        static ContentResult _firstUpResult;
        static VersionInfo _version;
        static Tuple<long, string>[] _migrations;
        static IMigrationService _migrationService;
    }
}