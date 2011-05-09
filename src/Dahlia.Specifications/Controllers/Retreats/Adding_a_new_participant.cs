using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Dahlia.Controllers;
using Dahlia.Models;
using Dahlia.Repositories;
using Dahlia.ViewModels;
using FizzWare.NBuilder;
using Machine.Specifications;
using MvcContrib.TestHelper;
using Rhino.Mocks;

namespace Dahlia.Specifications.Controllers.Retreats
{
    [Subject("Adding a new participant to a retreat")]
    public class When_showing_the_add_participant_screen_for_an_empty_retreat
    {
        Establish context = () =>
        {
            _retreatId = 1;

            _retreat = new Retreat();
            _retreatRepository = MockRepository.GenerateStub<IRetreatRepository>();
            _retreatRepository.Stub(x => x.GetById(_retreatId)).Return(_retreat);

            _beds = new[]
            {
                new Bed { Code = "Bed 1" },
                new Bed { Code = "Bed 2" },
                new Bed { Code = "Bed 3" }
            };
            var bedRepository = MockRepository.GenerateStub<IBedRepository>();
            bedRepository.Stub((x => x.GetAll())).Return(_beds);

            _controller = new RetreatController(_retreatRepository, bedRepository, null, null);
        };

        Because of = () =>
        {
            _viewResult = _controller.AddNewParticipant(_retreatId);
            _viewModel = (AddNewParticipantViewModel)_viewResult.ViewData.Model;
        };

        It should_display_the_add_participant_view = () =>
            _viewResult.AssertViewRendered().ForView("").WithViewData<AddNewParticipantViewModel>();

        It should_return_a_view_model_that_contains_the_retreat_id = () => _viewModel.RetreatId.ShouldEqual(_retreatId);

        It should_have_a_default_date_equal_to_today_for_the_received_date = () => _viewModel.Participant.DateReceived.ShouldEqual(DateTime.Today);

        It should_return_a_view_model_that_contains_all_beds = () => _viewModel.Beds.SequenceEqual(_beds).ShouldBeTrue();

        static int _retreatId;
        static ViewResult _viewResult;
        static AddNewParticipantViewModel _viewModel;
        static RetreatController _controller;
        static IRetreatRepository _retreatRepository;
        static Retreat _retreat;
        static IEnumerable<Bed> _beds;
    }

    [Subject("Adding a new participant to a retreat")]
    public class When_showing_the_add_participant_screen_for_a_partially_full_retreat
    {
        Establish context = () =>
        {
            _retreatId = 1;
            _retreatDate = new DateTime(2007, 12, 15);

            var registrations = Builder<Registration>.CreateListOfSize(1)
                .WhereAll().Have(x => x.Bed = Builder<Bed>.CreateNew().With(y => y.Code = "Bed 1").Build()).Build();

            var retreat = Builder<Retreat>.CreateNew()
                .WithConstructor(() => new Retreat(registrations))
                .With(x => x.StartDate = _retreatDate)
                .Build();

            _retreatRepo = MockRepository.GenerateStub<IRetreatRepository>();
            _retreatRepo.Stub(x => x.GetById(_retreatId)).Return(retreat);

            var beds = new[]
            {
                new Bed { Code = "Bed 1" },
                new Bed { Code = "Bed 2" },
                new Bed { Code = "Bed 3" }
            };
            var bedRepository = MockRepository.GenerateStub<IBedRepository>();
            bedRepository.Stub((x => x.GetAll())).Return(beds);

            _controller = new RetreatController(_retreatRepo, bedRepository, null, null);
        };

        Because of = () =>
        {
            _viewResult = _controller.AddNewParticipant(_retreatId);
            _viewModel = (AddNewParticipantViewModel)_viewResult.ViewData.Model;
        };

        It should_not_display_assigned_beds = () =>
            _viewModel.Beds.Any(bed => bed.Code == "Bed 1").ShouldBeFalse();

        static int _retreatId;
        static DateTime _retreatDate;
        static ViewResult _viewResult;
        static AddNewParticipantViewModel _viewModel;
        static RetreatController _controller;
        static IRetreatRepository _retreatRepo;
    }

    [Subject("Adding a new participant to a retreat")]
    public class When_showing_the_add_participant_screen_for_a_full_retreat
    {
        Establish context = () =>
        {
            _retreatRepo = MockRepository.GenerateStub<IRetreatRepository>();
            _retreatId = 1;
            _retreatDate = new DateTime(2007, 12, 15);

            var bedRepository = MockRepository.GenerateStub<IBedRepository>();
            bedRepository.Stub((x => x.GetAll())).Return(new List<Bed>());

            var registrations = Builder<Registration>.CreateListOfSize(29)
                .WhereAll().Have(x => x.Bed = Builder<Bed>.CreateNew().With(y => y.Code = "foo").Build()).Build();

            var retreat = Builder<Retreat>.CreateNew()
                .WithConstructor(() => new Retreat(registrations))
                .With(x => x.StartDate = _retreatDate)
                .Build();

            _retreatRepo.Stub(x => x.GetById(_retreatId)).Return(retreat);

            _controller = new RetreatController(_retreatRepo, bedRepository, null, null);
        };

        Because of = () =>
        {
            _viewResult = _controller.AddNewParticipant(_retreatId);
            _viewModel = (AddNewParticipantViewModel)_viewResult.ViewData.Model;
        };

        It should_disable_assigning_bed_codes = () =>
            _viewModel.RetreatIsFull.ShouldBeTrue();

        static int _retreatId;
        static DateTime _retreatDate;
        static ViewResult _viewResult;
        static AddNewParticipantViewModel _viewModel;
        static RetreatController _controller;
        static IRetreatRepository _retreatRepo;
    }

    [Subject("Adding a participant to a retreat")]
    public class When_the_user_saves_a_new_participant_for_the_retreat
    {
        Establish context = () =>
        {
            _invoker = new FakeControllerCommandInvoker();
            _controller = new RetreatController(_retreatRepository, null, _invoker, null);

            _viewModel = new AddNewParticipantViewModel { RetreatId = 123 };
            _invoker.ShouldSucceed = true;
        };

        Because of = () =>
        {
            _actionResult = _controller.AddNewParticipant(_viewModel);
        };

        It should_cause_the_participant_to_be_added = () =>
            _invoker.SuppliedInput.ShouldBeTheSameAs(_viewModel);

        It should_redirect_to_the_retreat_index_view = () =>
            _actionResult.AssertActionRedirect().ToAction<RetreatController>(c => c.Index(123));

        public static IRetreatRepository _retreatRepository;
        public static AddNewParticipantViewModel _viewModel;
        public static FakeControllerCommandInvoker _invoker;
        public static RetreatController _controller;
        public static ActionResult _actionResult;
    }
}
