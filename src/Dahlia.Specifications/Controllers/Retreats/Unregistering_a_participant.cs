using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Dahlia.Controllers;
using Dahlia.Models;
using Dahlia.Repositories;
using Dahlia.ViewModels;
using Machine.Specifications;
using MvcContrib.TestHelper;
using Rhino.Mocks;

namespace Dahlia.Specifications.Controllers.Retreats
{
    [Subject("Unregistering a participant")]
    public class when_the_user_starts_to_unregister_a_participant : RetreatControllerContext
    {
        Establish context = () =>
        {
            _retreat = new Retreat
            {
                Id = 123,
                StartDate = DateTime.Parse("1/1/2010")
            };
            _participant = new Participant
            {
                Id = 456,
                FirstName = "first",
                LastName = "last",
            };

            _retreat.AddParticipant(_participant, null);

            _retreatRepository.Stub(x => x.GetById(123)).Return(_retreat);

            _viewModel = new DeleteParticipantFromRetreatViewModel();
        };

        Because of = () =>
        {
            _actionResult = _controller.UnregisterParticipant(123, 456);
            _viewModel = _actionResult.AssertViewRendered().ForView("").WithViewData<DeleteParticipantFromRetreatViewModel>();
        };

        It should_display_the_confirmation_view_for_unregistering = () =>
            _actionResult.AssertViewRendered().ForView("").WithViewData<DeleteParticipantFromRetreatViewModel>();

        It should_create_a_view_model_containing_the_retreat_id = () =>
            _viewModel.RetreatId.ShouldEqual(_retreat.Id);

        It should_create_a_view_model_containing_the_participant_id = () =>
            _viewModel.ParticipantId.ShouldEqual(_participant.Id);

        It should_create_a_view_model_containing_the_participant_first_name = () =>
            _viewModel.FirstName.ShouldEqual(_participant.FirstName);

        It should_create_a_view_model_containing_the_participant_last_name = () =>
            _viewModel.LastName.ShouldEqual(_participant.LastName);

        It should_create_a_view_model_containing_the_participant_date_received = () =>
            _viewModel.RetreatDate.ShouldEqual(_retreat.StartDate);

        static Retreat _retreat;
        static Participant _participant;
    }

    [Subject("Unregistering a participant")]
    public class when_the_user_confirms_the_unregistration : RetreatControllerContext
    {
        Establish context = () =>
        {
            _viewModel = new DeleteParticipantFromRetreatViewModel {RetreatId = 123};
            _invoker.ShouldSucceed = true;
        };

        Because of = () =>
        {
            _actionResult = _controller.UnregisterParticipant(_viewModel);
        };

        It should_cause_the_participant_to_be_unregistered = () =>
            _invoker.SuppliedInput.ShouldBeTheSameAs(_viewModel);

        It should_redirect_to_the_retreat_index_view = () =>
            _actionResult.AssertActionRedirect().ToAction<RetreatController>(c => c.Index(123));
    }

    [Subject("Unregistering a participant")]
    public class when_the_user_deletes_an_invalid_participant : RetreatControllerContext
    {
        Establish context = () =>
        {
            _viewModel = new DeleteParticipantFromRetreatViewModel {RetreatId = 123};
            _invoker.ShouldSucceed = false;
        };

        Because of = () =>
        {
            _actionResult = _controller.UnregisterParticipant(_viewModel);
        };

        It should_redirect_to_the_retreat_index_view = () =>
            _actionResult.AssertActionRedirect().ToAction<RetreatController>(c => c.Index(123));
    }

    public class RetreatControllerContext
    {
        public static FakeControllerCommandInvoker _invoker;
        public static RetreatController _controller;
        public static ViewResult _viewResult;
        public static ActionResult _actionResult;
        public static DeleteParticipantFromRetreatViewModel _viewModel;
        public static IRetreatRepository _retreatRepository;

        Establish context = () =>
        {
            _retreatRepository = MockRepository.GenerateStub<IRetreatRepository>();
            _invoker = new FakeControllerCommandInvoker();
            _controller = new RetreatController(_retreatRepository, null, _invoker, null, null);
        };
    }
}
