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
    [Subject(typeof(EditParticipantCommand))]
    public class when_the_edit_participant_command_is_executed_and_succeeds : EditParticipantCommandContext
    {
        Establish context = () =>
        {
            _viewModel = new EditParticipantViewModel
            {
                Id = 123,
                FirstName = "Fred",
                LastName = "Flintstone",
                DateReceived = DateTime.Parse("1/1/2010"),
                Notes = "note",
                PhysicalStatus = PhysicalStatus.Limited
            };

            _participant = new Participant();
            _participantRepository.Stub(x => x.GetById(123)).Return(_participant);

            _retreatRepository.Stub(x => x.GetForParticipant(_participant.Id)).Return(new[] {new Retreat()});

        };

        Because of = () =>
            _isSuccessful = _command.Execute(_viewModel);

        It should_save_the_participant_to_the_repository = () =>
            _participantRepository.AssertWasCalled(x => x.Save(_participant));

        It should_save_the_participant_first_name = () =>
            _viewModel.FirstName.ShouldEqual(_participant.FirstName);

        It should_save_the_participant_last_name = () =>
            _viewModel.LastName.ShouldEqual(_participant.LastName);

        It should_save_the_participant_date_received = () =>
            _viewModel.DateReceived.ShouldEqual(_participant.DateReceived);

        It should_save_the_participant_notes = () =>
            _viewModel.Notes.ShouldEqual(_participant.Notes);

        It should_save_the_participant_physical_status = () =>
            _viewModel.PhysicalStatus.ShouldEqual(_participant.PhysicalStatus);

        It should_indicate_success = () =>
            _isSuccessful.ShouldBeTrue();

        static Participant _participant;
    }

    [Subject(typeof(EditParticipantCommand))]
    public class when_the_edit_participant_command_is_executed_and_fails : EditParticipantCommandContext
    {
        Establish context = () =>
            {
                _participantRepository.Stub(x => x.GetById(123)).Return(null);
                _retreatRepository.Stub(x => x.GetForParticipant(Arg<int>.Is.Anything)).Return(new[] {new Retreat()});
            };

        Because of = () =>
            _isSuccessful = _command.Execute(_viewModel);

        It should_indicate_failure = () =>
            _isSuccessful.ShouldBeFalse();
    }

    public class EditParticipantCommandContext
    {
        public static IParticipantRepository _participantRepository;
        public static IRetreatRepository _retreatRepository;
        public static IBedRepository _bedRepository;
        public static EditParticipantCommand _command;

        public static EditParticipantViewModel _viewModel;
        public static bool _isSuccessful;

        Establish context = () =>
        {
            _participantRepository = MockRepository.GenerateStub<IParticipantRepository>();
            _bedRepository = MockRepository.GenerateStub<IBedRepository>();
            _retreatRepository = MockRepository.GenerateStub<IRetreatRepository>();
            _bedRepository.Stub(x => x.GetAll()).Return(new[] {new Bed()});
            

            _command = new EditParticipantCommand(_participantRepository, _retreatRepository,_bedRepository );
            _viewModel = new EditParticipantViewModel();
        };
    }
}
