using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dahlia.Commands;
using Dahlia.Models;
using Dahlia.Repositories;
using Machine.Specifications;
using Rhino.Mocks;

namespace Dahlia.Specifications.Commands
{
    [Subject(typeof(CreateRetreatCommand))]
    public class when_the_create_retreat_command_is_executed_and_succeeds : CreateRetreatCommandContext
    {
        Establish context = () =>
        {
            _viewModel = new Retreat
            {
                Description = "retreat"
            };
        };

        Because of = () =>
            _isSuccessful = _command.Execute(_viewModel);

        It should_save_the_participant_to_the_repository = () =>
            _repository.AssertWasCalled(x => x.Save(Arg<Retreat>.Is.Anything));

        // TODO: we need to get the saved participant and verify that it has the correct attributes

        It should_indicate_success = () =>
            _isSuccessful.ShouldBeTrue();
    }

    [Subject(typeof(CreateRetreatCommand))]
    public class when_the_create_retreat_command_is_executed_and_fails : CreateRetreatCommandContext
    {
        Establish context = () =>
            _repository.Stub(x => x.Save(Arg<Retreat>.Is.Anything)).Throw(new Exception());

        Because of = () =>
            _isSuccessful = _command.Execute(_viewModel);

        It should_indicate_failure = () =>
            _isSuccessful.ShouldBeFalse();

        It should_contain_an_exception = () =>
            _command.Exception.ShouldNotBeNull();
    }

    public class CreateRetreatCommandContext
    {
        public static IRetreatRepository _repository;
        public static CreateRetreatCommand _command;

        public static Retreat _viewModel;
        public static bool _isSuccessful;

        Establish context = () =>
        {
            _repository = MockRepository.GenerateStub<IRetreatRepository>();
            _command = new CreateRetreatCommand(_repository);
            _viewModel = new Retreat();
        };
    }
}
