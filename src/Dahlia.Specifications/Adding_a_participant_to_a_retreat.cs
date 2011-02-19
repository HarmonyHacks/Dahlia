using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Dahlia.Controllers;
using Machine.Specifications;

namespace Dahlia.Specifications.Adding_a_participant_to_a_retreat
{
    public class When_showing_the_add_participant_screen_for_a_retreat
    {
        Establish context = () =>
                            {
                                _RetreatDate = new DateTime(2007, 12, 15);
                                _Controller = new ParticipantController();
                            };
        
        Because of = () => _ViewResult = _Controller.AddToRetreat(_RetreatDate);

        It should_return_a_view_result_for_the_add_participant_view = () => _ViewResult.ViewName.ShouldEqual("AddToRetreat");

        static DateTime _RetreatDate;
        static ViewResult _ViewResult;
        static AddParticipantToRetreatViewModel _ViewModel;
        static ParticipantController _Controller;
    }

    public class AddParticipantToRetreatViewModel
    {
    }
}
