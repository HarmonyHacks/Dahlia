using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Dahlia.Controllers;
using Dahlia.ViewModels;
using Machine.Specifications;

namespace Dahlia.Specifications.Adding_a_participant_to_a_retreat
{
    public class When_showing_the_add_participant_screen_for_a_retreat
    {
        Establish context = () =>
                            {
                                _retreatDate = new DateTime(2007, 12, 15);
                                _controller = new ParticipantController();
                            };

        private Because of = () =>
                                 {
                                     _viewResult = _controller.AddToRetreat(_retreatDate);
                                     _viewModel = (AddParticipantToRetreatViewModel) _viewResult.ViewData.Model;
                                 };

        It should_return_a_view_result_for_the_add_participant_view = () => _viewResult.ViewName.ShouldEqual("AddToRetreat");

        It should_return_a_view_model_that_contains_the_retreat_date = () => _viewModel.RetreatDate.ShouldEqual(_retreatDate);

        It should_have_a_default_date_equal_to_today_for_the_received_date = () => _ViewModel.DateReceived.ShouldEqual(DateTime.Today);

        static DateTime _retreatDate;
        static ViewResult _viewResult;
        static AddParticipantToRetreatViewModel _viewModel;
        static ParticipantController _controller;
    }
}
