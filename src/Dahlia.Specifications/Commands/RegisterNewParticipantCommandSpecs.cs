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
    [Subject(typeof(RegisterNewParticipantCommand))]
    public class when_the_register_new_participant_command_is_executed_and_succeeds : RegisterNewParticipantCommandContext
    {
        Establish context = () =>
        {
            _viewModel = new AddNewParticipantViewModel
            {
                RetreatId = 123,
                BedCode = "bedcode",
                Participant = new CreateParticipantViewModel
                {
                    FirstName = "Fred",
                    LastName = "Flintstone",
                    DateReceived = DateTime.Parse("1/1/2010"),
                    Notes = "note",
                    PhysicalStatus = PhysicalStatus.Limited
                }
            };

            _participantRepository = MockRepository.GenerateStub<IParticipantRepository>();

            _retreat = new Retreat();
            _retreatRepository.Stub(x => x.GetById(123)).Return(_retreat);

            _bed = new Bed {Code = "bedcode"};
            _bedRepository.Stub(x => x.GetBy("bedcode")).Return(_bed);
        };

        Because of = () =>
            _isSuccessful = _command.Execute(_viewModel);

        It should_indicate_success = () =>
            _isSuccessful.ShouldBeTrue();

        It should_create_a_new_participant = () =>
            _createParticipantCommand.CreatedParticipant.ShouldNotBeNull();

        It should_register_the_participant_with_the_correct_bed_code = () =>
            _retreat.Registrations.Any(r => r.Participant == _createParticipantCommand.CreatedParticipant && r.Bed == _bed).ShouldBeTrue();

        static Participant _participant;
        static Retreat _retreat;
        static Bed _bed;
        static IParticipantRepository _participantRepository;
    }


    [Subject(typeof(RemoveParticipantFromRetreatCommand))]
    public class when_the_register_new_participant_command_is_executed_and_fails : RegisterNewParticipantCommandContext
    {
        Establish context = () =>
            _bedRepository.Stub(x => x.GetBy("bedcode")).Return(null);

        Because of = () =>
            _isSuccessful = _command.Execute(_viewModel);

        It should_indicate_failure = () =>
            _isSuccessful.ShouldBeFalse();
    }

    public class RegisterNewParticipantCommandContext
    {
        public static IRetreatRepository _retreatRepository;
        public static IBedRepository _bedRepository;
        public static IParticipantRepository _participantRepository;
        public static CreateParticipantCommand _createParticipantCommand;
        public static RegisterNewParticipantCommand _command;

        public static AddNewParticipantViewModel _viewModel;
        public static bool _isSuccessful;

        Establish context = () =>
        {
            _retreatRepository = MockRepository.GenerateStub<IRetreatRepository>();
            _bedRepository = MockRepository.GenerateStub<IBedRepository>();
            _participantRepository = MockRepository.GenerateStub<IParticipantRepository>();
            _createParticipantCommand = new CreateParticipantCommand(_participantRepository);
            _command = new RegisterNewParticipantCommand(_retreatRepository, _bedRepository, _createParticipantCommand);
            _viewModel = new AddNewParticipantViewModel();
        };
    }
}
