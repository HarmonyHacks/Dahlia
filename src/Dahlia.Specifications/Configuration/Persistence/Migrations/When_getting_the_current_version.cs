using Dahlia.Configuration.Persistence.Migrations;
using Dahlia.Models;
using Dahlia.Repositories;
using Machine.Specifications;
using NHibernate;
using Rhino.Mocks;

namespace Dahlia.Specifications.Configuration.Persistence.Migrations
{
    public class When_getting_the_current_version
    {
        Establish context = () =>
        {
            _version = new VersionInfo {Version = 666};
            _versionRepo = MockRepository.GenerateStub<IVersionRepository>();
            _item = new MigrationInformation(_versionRepo);

            _versionRepo.Stub(x => x.GetCurrent()).Return(_version);
        };

        Because of = () =>
        { _result = _item.CurrentVersion(); };

        It should_get_a_result = () =>
            _result.ShouldNotBeNull();

        It should_be_the_correct_version = () =>
            _result.Version.ShouldEqual(_version.Version);

        static MigrationInformation _item;
        static VersionInfo _result;
        static IVersionRepository _versionRepo;
        static VersionInfo _version;
    }
}