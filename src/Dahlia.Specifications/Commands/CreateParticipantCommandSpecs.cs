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
    [Subject(typeof(CreateParticipantCommand))]
    public class when_the_create_participant_command_is_executed_and_succeeds : CreateParticipantCommandContext
    {
        Establish context = () =>
        {
            _viewModel = new CreateParticipantViewModel
            {
                FirstName = "Fred",
                LastName = "Flintstone",
                DateReceived = DateTime.Parse("1/1/2010"),
                Notes = "note",
                PhysicalStatus = PhysicalStatus.Limited
            };

            _participant = new Participant();
        };

        Because of = () =>
            _isSuccessful = _command.Execute(_viewModel);

        It should_save_the_participant_to_the_repository = () =>
            _repository.AssertWasCalled(x => x.Save(Arg<Participant>.Is.Anything));

        // TODO: we need to get the saved participant and verify that it has the correct attributes

        It should_indicate_success = () =>
            _isSuccessful.ShouldBeTrue();

        static Participant _participant;
    }

    [Subject(typeof(CreateParticipantCommand))]
    public class when_the_create_participant_command_is_executed_and_fails : CreateParticipantCommandContext
    {
        Establish context = () =>
            _repository.Stub(x => x.Save(Arg<Participant>.Is.Anything)).Throw(new Exception());

        Because of = () =>
            _isSuccessful = _command.Execute(_viewModel);

        It should_indicate_failure = () =>
            _isSuccessful.ShouldBeFalse();

        It should_contain_an_exception = () =>
            _command.Exception.ShouldNotBeNull();
    }

    public class CreateParticipantCommandContext
    {
        public static IParticipantRepository _repository;
        public static CreateParticipantCommand _command;

        public static CreateParticipantViewModel _viewModel;
        public static bool _isSuccessful;

        Establish context = () =>
        {
            _repository = MockRepository.GenerateStub<IParticipantRepository>();
            _command = new CreateParticipantCommand(_repository);
            _viewModel = new CreateParticipantViewModel();
        };
    }
}
