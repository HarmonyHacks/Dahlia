using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Dahlia.Controllers;
using Dahlia.Models;
using Dahlia.Repositories;
using Dahlia.Services;
using Dahlia.ViewModels;
using FizzWare.NBuilder;
using Machine.Specifications;
using Rhino.Mocks;

namespace Dahlia.Specifications
{
    [Subject("Adding a participant to a retreat")]
    public class When_showing_the_add_participant_screen_for_a_retreat
    {
        Establish context = () =>
        {
            _retreatId = 1;
            _retreatRepository = MockRepository.GenerateStub<IRetreatRepository>();
            var bedRepository = MockRepository.GenerateStub<IBedRepository>();
            _controller = new ParticipantController(_retreatRepository, null, bedRepository, null);
            _beds = new[]
            {
                new Bed { Code = "Bed 1" },
                new Bed { Code = "Bed 2" },
                new Bed { Code = "Bed 3" }
            };

            _retreat = new Retreat { };
            _retreatRepository.Stub(x => x.GetById(_retreatId)).Return(_retreat);
            bedRepository.Stub((x => x.GetAll())).Return(_beds);
        };

        Because of = () =>
        {
            _viewResult = _controller.AddToRetreat(_retreatId);
            _viewModel = (AddParticipantToRetreatViewModel) _viewResult.ViewData.Model;
        };

        It should_return_a_view_result_for_the_add_participant_view = () => _viewResult.ViewName.ShouldEqual("AddToRetreat");

        It should_return_a_view_model_that_contains_the_retreat_id = () => _viewModel.RetreatId.ShouldEqual(_retreatId);

        It should_have_a_default_date_equal_to_today_for_the_received_date = () => _viewModel.DateReceived.ShouldEqual(DateTime.Today);

        It should_return_a_view_model_that_contains_all_beds = () => _viewModel.Beds.SequenceEqual(_beds).ShouldBeTrue();

        static int _retreatId;
        static ViewResult _viewResult;
        static AddParticipantToRetreatViewModel _viewModel;
        static ParticipantController _controller;
        static IRetreatRepository _retreatRepository;
        static Retreat _retreat;
        static IEnumerable<Bed> _beds;
    }

    [Subject("Adding a participant to a retreat")]
    public class When_showing_the_add_participant_screen_for_a_full_retreat
    {
        Establish context = () =>
        {
            _retreatRepo = MockRepository.GenerateStub<IRetreatRepository>();
            _retreatId = 1;
            _retreatDate = new DateTime(2007, 12, 15);
            var bedRepository = MockRepository.GenerateStub<IBedRepository>();
            _controller = new ParticipantController(_retreatRepo, null, bedRepository, null);

            var registrations = Builder<Registration>.CreateListOfSize(29)
                .WhereAll().Have(x => x.Bed = Builder<Bed>.CreateNew().With(y => y.Code = "foo").Build()).Build();

            var retreat = Builder<Retreat>.CreateNew()
                .With(x => x.StartDate = _retreatDate)
                .And(x => x.Registrations = registrations)
                .Build();

            _retreatRepo.Stub(x => x.GetById(_retreatId)).Return(retreat);

        };

        Because of = () =>
        {
            _viewResult = _controller.AddToRetreat(_retreatId);
            _viewModel = (AddParticipantToRetreatViewModel)_viewResult.ViewData.Model;
        };

        It should_disable_assigning_bed_codes = () =>
            _viewModel.RetreatIsFull.ShouldBeTrue();

        static int _retreatId;
        static DateTime _retreatDate;
        static ViewResult _viewResult;
        static AddParticipantToRetreatViewModel _viewModel;
        static ParticipantController _controller;
        static IRetreatRepository _retreatRepo;
    }

    [Subject("Adding a participant to a retreat")]
    public class When_posting_back_from_the_add_participant_screen_for_a_retreat
    {
        Establish context = () =>
        {
            var bed = new Bed { Code = "bedcode" };
            var retreatDate = new DateTime(2007, 12, 15);
            _viewModel = new AddParticipantToRetreatViewModel
                         {
                             RetreatDate = retreatDate,
                             BedCode = bed.Code,
                             FirstName = "firstbob",
                             LastName = "lastmartin",
                             DateReceived = DateTime.Today,
                             PhysicalStatus = PhysicalStatus.Limited,
                             Notes = "yada yada yada...",
                             Save = "Save participant",
                         };
            _retreatDate = retreatDate;
            
            _retreatRepository = MockRepository.GenerateStub<IRetreatRepository>();
            var bedRepository = MockRepository.GenerateStub<IBedRepository>();
            _controller = new ParticipantController(_retreatRepository, null, bedRepository, null);

            _retreat = new Retreat{ StartDate = retreatDate };
            _retreatRepository.Stub(x => x.Get(retreatDate)).Return(_retreat);
            bedRepository.Stub(x => x.GetBy(bed.Code)).Return(bed);
        };

        Because of = () => _controller.DoAddToRetreat(_viewModel);

        It should_save_the_retreat = () => 
            _retreatRepository.AssertWasCalled(x => x.Save(_retreat));
        
        It should_add_the_participant_to_the_retreat = () => 
            _retreat.Registrations.Count.ShouldEqual(1);
        
        It should_give_the_participant_the_right_first_name = () => 
            _retreat.Registrations[0].Participant.FirstName.ShouldEqual(_viewModel.FirstName);
        
        It should_give_the_participant_the_right_last_name = () => 
            _retreat.Registrations[0].Participant.LastName.ShouldEqual(_viewModel.LastName);
        
        It should_give_the_participant_the_right_date_recieved = () => 
            _retreat.Registrations[0].Participant.DateReceived.ShouldEqual(_viewModel.DateReceived);
        
        It should_give_the_participant_the_right_notes = () => 
            _retreat.Registrations[0].Participant.Notes.ShouldEqual(_viewModel.Notes);

        It should_assign_the_right_bed_code = () => 
            _retreat.Registrations[0].Bed.Code.ShouldEqual(_viewModel.BedCode);
        
        It should_assign_the_retreat = () => 
            _retreat.Registrations[0].Retreat.StartDate.ShouldEqual(_retreat.StartDate);

        It should_assign_the_physical_status = () =>
            _retreat.Registrations[0].Participant.PhysicalStatus.ShouldEqual(_viewModel.PhysicalStatus);

        static DateTime _retreatDate;
        static AddParticipantToRetreatViewModel _viewModel;
        static ParticipantController _controller;
        static IRetreatRepository _retreatRepository;
        static Retreat _retreat;
    }

    [Subject("Adding a participant to a retreat")]
    public class When_posting_back_a_search_from_the_add_participant_screen_for_a_retreat
    {
        Establish context = () =>
        {
            var retreatDate = new DateTime(2007, 12, 15);
            _viewModel = new AddParticipantToRetreatViewModel
            {
                RetreatDate = retreatDate,
                FirstName = "bob",
                LastName = "fred",
                DateReceived = DateTime.Today,
                Search = "Search",
            };
            _retreatDate = retreatDate;
            _firstDateReceived = new DateTime(2007, 12, 31);

            _retreatRepository = MockRepository.GenerateStub<IRetreatRepository>();
            _participantRepository = MockRepository.GenerateStub<IParticipantRepository>();

            _queryOutput = new[]
                           {
                               new Participant {FirstName = "Bobathon", LastName = "fred", DateReceived = _firstDateReceived },
                               new Participant {FirstName = "bob", LastName = "Fredding"},
                           };
            _participantRepository.Stub(x => x.WithNameLike("bob", "fred")).Return(_queryOutput);

            var urlMapper = MockRepository.GenerateStub<IUrlMapper>();
            _controller = new ParticipantController(_retreatRepository, _participantRepository, null, urlMapper);

            _retreat = new Retreat { StartDate = retreatDate };
            _retreatRepository.Stub(x => x.Get(retreatDate)).Return(_retreat);
        };

        Because of = () => _result = _controller.DoAddToRetreat(_viewModel);

        It should_redirect_back_to_the_add_to_retreat_view =
            () => ((RedirectToRouteResult) _result).RouteValues["action"].ShouldEqual("AddToRetreat");

        It should_supply_a_search_results_object_in_the_temp_data =
            () => _controller.TempData["searchResults"].ShouldBe(typeof (AddParticipantToRetreatSearchResultsViewModel));

        It should_have_the_correct_name =
            () => ((AddParticipantToRetreatSearchResultsViewModel) _controller.TempData["searchResults"]).SearchResults
                .First().Name.ShouldEqual("Bobathon fred");

        It should_have_the_correct_date_received =
            () => ((AddParticipantToRetreatSearchResultsViewModel) _controller.TempData["searchResults"]).SearchResults
                .First().DateReceived.ShouldEqual(_firstDateReceived);

        It should_have_the_correct_number_of_results =
            () => ((AddParticipantToRetreatSearchResultsViewModel) _controller.TempData["searchResults"])
                .SearchResults.Count.ShouldEqual(2);

        static DateTime _retreatDate;
        static AddParticipantToRetreatViewModel _viewModel;
        static ParticipantController _controller;
        static IRetreatRepository _retreatRepository;
        static Retreat _retreat;
        static ActionResult _result;
        static IParticipantRepository _participantRepository;
        static Participant[] _queryOutput;
        static DateTime _firstDateReceived;
    }

    [Subject("Adding a participant to a retreat")]
    public class When_adding_a_participant_to_the_list
    {
        Because of = () => 
            _viewModel = new AddParticipantToRetreatViewModel();

        It defaults_to_the_participant_being_unlimited = () =>
            _viewModel.PhysicalStatus.ShouldEqual(PhysicalStatus.Unlimited);

        static AddParticipantToRetreatViewModel _viewModel;
    }
}
