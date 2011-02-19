using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Dahlia.Controllers;
using Dahlia.Models;
using Dahlia.Services;
using Dahlia.ViewModels;
using Machine.Specifications;
using Rhino.Mocks;

namespace Dahlia.Specifications
{
    [Subject("Adding a participant to a retreat")]
    public class When_showing_the_add_participant_screen_for_a_retreat
    {
        Establish context = () =>
                            {
                                _retreatDate = new DateTime(2007, 12, 15);
                                _controller = new ParticipantController(null);
                            };

        Because of = () =>
                                 {
                                     _viewResult = _controller.AddToRetreat(_retreatDate);
                                     _viewModel = (AddParticipantToRetreatViewModel) _viewResult.ViewData.Model;
                                 };

        It should_return_a_view_result_for_the_add_participant_view = () => _viewResult.ViewName.ShouldEqual("AddToRetreat");

        It should_return_a_view_model_that_contains_the_retreat_date = () => _viewModel.RetreatDate.ShouldEqual(_retreatDate);

        It should_have_a_default_date_equal_to_today_for_the_received_date = () => _viewModel.DateReceived.ShouldEqual(DateTime.Today);

        static DateTime _retreatDate;
        static ViewResult _viewResult;
        static AddParticipantToRetreatViewModel _viewModel;
        static ParticipantController _controller;
    }

    [Subject("Adding a participant to a retreat")]
    public class When_posting_back_from_the_add_participant_screen_for_a_retreat
    {
        Establish context = () =>
        {
            _viewModel = new AddParticipantToRetreatViewModel
                         {
                             RetreatDate = new DateTime(2007, 12, 15),
                             BedCode = "bedcode",
                             FirstName = "firstbob",
                             LastName = "lastmartin",
                             DateReceived = DateTime.Today,
                             Notes = "yada yada yada...",
                         };
            _retreatDate = new DateTime(2007, 12, 15);
            
            _retreatParticipantAdder = MockRepository.GenerateStub<IRetreatParticipantAdder>();
            _controller = new ParticipantController(_retreatParticipantAdder);
        };

        Because of = () => _controller.DoAddToRetreat(_viewModel);

        It should_add_the_participant = () => _retreatParticipantAdder.AssertWasCalled(x => x.AddParticipantToRetreat(Arg<DateTime>.Is.Equal(_retreatDate), Arg<Participant>.Is.Anything));

        static DateTime _retreatDate;
        static AddParticipantToRetreatViewModel _viewModel;
        static ParticipantController _controller;
        static IRetreatParticipantAdder _retreatParticipantAdder;
    }
}
