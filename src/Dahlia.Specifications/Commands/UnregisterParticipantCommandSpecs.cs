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
    [Subject(typeof(RemoveParticipantFromRetreatCommand))]
    public class when_the_remove_participant_command_is_executed_and_succeeds : RemoveParticipantCommandContext
    {
        Establish context = () =>
        {
            _viewModel = new RemoveParticipantFromRetreatViewModel
            {
                RetreatId = 123,
                ParticipantId = 456
            };

            _participant = new Participant {Id = 456};
            _retreat = new Retreat();
            _repository.Stub(x => x.GetById(123)).Return(_retreat);

            _retreat.AddParticipant(_participant, new Bed {Code = "bedcode"});
        };

        Because of = () =>
            _isSuccessful = _command.Execute(_viewModel);

        It should_indicate_success = () =>
            _isSuccessful.ShouldBeTrue();

        It should_remove_the_participant_from_the_retreat = () =>
            _retreat.Registrations.Any(r => r.Participant == _participant).ShouldBeFalse();

        static Participant _participant;
        static Retreat _retreat;
    }


    [Subject(typeof(RemoveParticipantFromRetreatCommand))]
    public class when_the_remove_participant_command_is_executed_and_fails : RemoveParticipantCommandContext
    {
        Establish context = () =>
            _repository.Stub(x => x.GetById(123)).Return(null);

        Because of = () =>
            _isSuccessful = _command.Execute(_viewModel);

        It should_indicate_failure = () =>
            _isSuccessful.ShouldBeFalse();
    }

    public class RemoveParticipantCommandContext
    {
        public static IRetreatRepository _repository;
        public static RemoveParticipantFromRetreatCommand _command;

        public static RemoveParticipantFromRetreatViewModel _viewModel;
        public static bool _isSuccessful;

        Establish context = () =>
        {
            _repository = MockRepository.GenerateStub<IRetreatRepository>();
            _command = new RemoveParticipantFromRetreatCommand(_repository);
            _viewModel = new RemoveParticipantFromRetreatViewModel();
        };
    }
}
