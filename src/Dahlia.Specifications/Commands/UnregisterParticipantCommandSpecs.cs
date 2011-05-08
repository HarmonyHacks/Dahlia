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
    [Subject(typeof(UnregisterParticipantCommand))]
    public class when_the_unregister_participant_command_is_executed_and_succeeds : UnregisterParticipantCommandContext
    {
        Establish context = () =>
        {
            _viewModel = new DeleteParticipantFromRetreatViewModel
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

        It should_unregister_the_participant_from_the_retreat = () =>
            _retreat.Registrations.Any(r => r.Participant == _participant).ShouldBeFalse();

        static Participant _participant;
        static Retreat _retreat;
    }


    [Subject(typeof(UnregisterParticipantCommand))]
    public class when_the_unregister_participant_command_is_executed_and_fails : UnregisterParticipantCommandContext
    {
        Establish context = () =>
            _repository.Stub(x => x.GetById(123)).Return(null);

        Because of = () =>
            _isSuccessful = _command.Execute(_viewModel);

        It should_indicate_failure = () =>
            _isSuccessful.ShouldBeFalse();
    }

    public class UnregisterParticipantCommandContext
    {
        public static IRetreatRepository _repository;
        public static UnregisterParticipantCommand _command;

        public static DeleteParticipantFromRetreatViewModel _viewModel;
        public static bool _isSuccessful;

        Establish context = () =>
        {
            _repository = MockRepository.GenerateStub<IRetreatRepository>();
            _command = new UnregisterParticipantCommand(_repository);
            _viewModel = new DeleteParticipantFromRetreatViewModel();
        };
    }
}
