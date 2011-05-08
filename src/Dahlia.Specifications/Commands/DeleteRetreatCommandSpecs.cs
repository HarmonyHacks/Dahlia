using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dahlia.Commands;
using Dahlia.Models;
using Dahlia.Repositories;
using Dahlia.ViewModels;
using Machine.Specifications;
using Rhino.Mocks;

namespace Dahlia.Specifications.Commands
{
    [Subject(typeof(DeleteRetreatCommand))]
    public class when_the_delete_retreat_command_is_executed_and_succeeds : DeleteRetreatCommandContext
    {
        Establish context = () =>
        {
            _viewModel = new DeleteRetreatViewModel
            {
                Id = 123,
                Description = "retreat",
                StartDate = DateTime.Parse("1/1/2011")
            };
        };

        Because of = () =>
            _isSuccessful = _command.Execute(_viewModel);

        It should_delete_the_retreat_from_the_repository = () =>
            _repository.AssertWasCalled(x => x.DeleteById(123));

        It should_indicate_success = () =>
            _isSuccessful.ShouldBeTrue();
    }

    [Subject(typeof(DeleteRetreatCommand))]
    public class when_the_delete_retreat_command_is_executed_and_fails : DeleteRetreatCommandContext
    {
        Establish context = () =>
            _repository.Stub(x => x.DeleteById(Arg<int>.Is.Anything)).Throw(new Exception());

        Because of = () =>
            _isSuccessful = _command.Execute(_viewModel);

        It should_indicate_failure = () =>
            _isSuccessful.ShouldBeFalse();

        It should_contain_an_exception = () =>
            _command.Exception.ShouldNotBeNull();
    }

    public class DeleteRetreatCommandContext
    {
        public static IRetreatRepository _repository;
        public static DeleteRetreatCommand _command;

        public static DeleteRetreatViewModel _viewModel;
        public static bool _isSuccessful;

        Establish context = () =>
        {
            _repository = MockRepository.GenerateStub<IRetreatRepository>();
            _command = new DeleteRetreatCommand(_repository);
            _viewModel = new DeleteRetreatViewModel();
        };
    }
}
