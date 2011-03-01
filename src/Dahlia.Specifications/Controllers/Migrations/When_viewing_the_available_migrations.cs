using System;
using System.Linq;
using System.Web.Mvc;
using Dahlia.Configuration.Persistence.Migrations;
using Dahlia.Controllers;
using Dahlia.Models;
using Dahlia.Services;
using Dahlia.ViewModels;
using Machine.Specifications;
using Rhino.Mocks;

namespace Dahlia.Specifications.Controllers.Migrations
{
    public class When_viewing_the_available_migrations
    {
        Establish context = () =>
        {
            _version = new VersionInfo {Version = 1};
            _migrations = new[] {Tuple.Create(1L,"foo")};
            _migrationInformation = MockRepository.GenerateStub<IMigrationInformation>();
            _migrationInformation.Stub(x => x.CurrentVersion()).Return(_version);
            _migrationInformation.Stub(x => x.GetMigrations()).Return(_migrations);
            _controller = new MigrateController(null,_migrationInformation);
        };

        Because of = () =>
        {
            _viewResult = _controller.Index();
            _viewModel = (MigrationsViewModel) _viewResult.ViewData.Model;
        };

        It should_return_a_view_model = () =>
            _viewModel.ShouldNotBeNull();

        It should_display_the_current_version_number = () =>
            _viewModel.CurrentVersion.ShouldEqual(_version.Version);

        It should_display_the_available_versions = () =>
            _viewModel.AvailableVersions.Count().ShouldBeGreaterThan(0);

        static IMigrationInformation _migrationInformation;
        static MigrateController _controller;
        static ViewResult _viewResult;
        static MigrationsViewModel _viewModel;
        static VersionInfo _version;
        static Tuple<long, string>[] _migrations;
    }
}