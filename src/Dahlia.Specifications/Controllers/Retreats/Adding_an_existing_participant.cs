using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Dahlia.Commands;
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
    [Subject("Adding an existing participant to a retreat")]
    public class When_showing_the_choose_bed_code_screen_for_an_empty_retreat
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

            _controller = new RetreatController(_retreatRepository, null, bedRepository, null, null);
        };

        Because of = () =>
        {
            _viewResult = _controller.AddParticipantChooseBedCode(_retreatId, 123);
            _viewModel = (AddParticipantChooseBedCodeViewModel) _viewResult.ViewData.Model;
        };

        It should_display_the_choosebed_code_view = () =>
            _viewResult.AssertViewRendered().ForView("").WithViewData<AddParticipantChooseBedCodeViewModel>();

        It should_return_a_view_model_that_contains_the_retreat_id = () =>
            _viewModel.RetreatId.ShouldEqual(_retreatId);

        It should_return_a_view_model_that_contains_the_participant_id = () =>
            _viewModel.ParticipantId.ShouldEqual(123);

        It should_return_a_view_model_that_contains_all_beds = () =>
            _viewModel.BedCodeList.Skip(1).SequenceEqual(_beds.Select(b => b.Code)).ShouldBeTrue();

        It should_return_a_view_model_that_contains_an_option_for_no_bed = () =>
            _viewModel.BedCodeList.First().ShouldEqual("(none)");

        static int _retreatId;
        static ViewResult _viewResult;
        static AddParticipantChooseBedCodeViewModel _viewModel;
        static RetreatController _controller;
        static IRetreatRepository _retreatRepository;
        static Retreat _retreat;
        static IEnumerable<Bed> _beds;
    }

    [Subject("Adding an existing participant to a retreat")]
    public class When_showing_the_choose_bed_code_screen_for_a_partially_full_retreat
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

            _controller = new RetreatController(_retreatRepo, null, bedRepository, null, null);
        };

        Because of = () =>
        {
            _viewResult = _controller.AddParticipantChooseBedCode(_retreatId, 123);
            _viewModel = (AddParticipantChooseBedCodeViewModel)_viewResult.ViewData.Model;
        };

        It should_not_display_assigned_beds = () =>
            _viewModel.BedCodeList.Any(bed => bed == "Bed 1").ShouldBeFalse();

        It should_return_a_view_model_that_contains_an_option_for_no_bed = () =>
            _viewModel.BedCodeList.First().ShouldEqual("(none)");

        static int _retreatId;
        static DateTime _retreatDate;
        static ViewResult _viewResult;
        static AddParticipantChooseBedCodeViewModel _viewModel;
        static RetreatController _controller;
        static IRetreatRepository _retreatRepo;
    }

    [Subject("Adding an existing participant to a retreat")]
    public class When_showing_the_choose_bed_code_screen_for_a_full_retreat
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

            _controller = new RetreatController(_retreatRepo, null, bedRepository, null, null);
        };

        Because of = () =>
        {
            _viewResult = _controller.AddParticipantChooseBedCode(_retreatId, 123);
            _viewModel = (AddParticipantChooseBedCodeViewModel)_viewResult.ViewData.Model;
        };

        It should_not_display_assigned_beds = () =>
            _viewModel.BedCodeList.Count().ShouldEqual(1);

        It should_return_a_view_model_that_contains_an_option_for_no_bed = () =>
            _viewModel.BedCodeList.First().ShouldEqual("(none)");

        static int _retreatId;
        static DateTime _retreatDate;
        static ViewResult _viewResult;
        static AddParticipantChooseBedCodeViewModel _viewModel;
        static RetreatController _controller;
        static IRetreatRepository _retreatRepo;
    }

    [Subject("Adding an existing participant to a retreat")]
    public class When_the_user_saves_a_bed_code_assignment
    {
        Establish context = () =>
        {
            _invoker = new FakeControllerCommandInvoker();
            _controller = new RetreatController(_retreatRepository, null, null, _invoker, null);

            _viewModel = new AddParticipantChooseBedCodeViewModel { RetreatId = 123 };
            _invoker.ShouldSucceed = true;
        };

        Because of = () =>
        {
            _actionResult = _controller.AddParticipantChooseBedCode(_viewModel);
        };

        It should_invoke_an_add_existing_participant_command = () =>
            _invoker.SuppliedCommandType.ShouldEqual(typeof (AddExistingParticipantToRetreatCommand));

        It should_cause_the_participant_to_be_added = () =>
            _invoker.SuppliedInput.ShouldBeTheSameAs(_viewModel);

        It should_redirect_to_the_retreat_index_view = () =>
            _actionResult.AssertActionRedirect().ToAction<RetreatController>(c => c.Index(123));

        public static IRetreatRepository _retreatRepository;
        public static AddParticipantChooseBedCodeViewModel _viewModel;
        public static FakeControllerCommandInvoker _invoker;
        public static RetreatController _controller;
        public static ActionResult _actionResult;
    }

    [Subject("Adding an existing participant to a retreat")]
    public class When_the_user_cancels_the_bed_code_assignment
    {
        Establish context = () =>
        {
            _controller = new RetreatController(_retreatRepository, null, null, _invoker, null);

            _viewModel = new AddParticipantChooseBedCodeViewModel
            {
                RetreatId = 123,
                Cancel = "Cancel"
            };
        };

        Because of = () =>
        {
            _actionResult = _controller.AddParticipantChooseBedCode(_viewModel);
        };

        It should_redirect_to_the_retreat_index_view = () =>
            _actionResult.AssertActionRedirect().ToAction<RetreatController>(c => c.AddParticipant(123)).WithParameter("retreatId", 123);

        public static IRetreatRepository _retreatRepository;
        public static AddParticipantChooseBedCodeViewModel _viewModel;
        public static FakeControllerCommandInvoker _invoker;
        public static RetreatController _controller;
        public static ActionResult _actionResult;
    }
}
