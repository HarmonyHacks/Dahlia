using System;
using System.Web.Mvc;
using Dahlia.Controllers;
using Dahlia.Models;
using Dahlia.Repositories;
using Dahlia.ViewModels;
using Machine.Specifications;
using MvcContrib.TestHelper;
using Rhino.Mocks;

namespace Dahlia.Specifications.Deleting_a_retreat
{
    [Subject("Deleting A Retreat")]
    public class when_getting_the_view_to_ask_the_user_about_deleting_a_retreat
    {
        Establish context = () =>
        {
            _repo = MockRepository.GenerateStub<IRetreatRepository>();
            _repo.Stub(x => x.GetById(100)).Return(new Retreat {Id = 100,StartDate = new DateTime(2011,1,1)});
            _controller = new RetreatController(_repo, null);
        };

        Because of = () => _result = (ViewResult)_controller.Delete(100);

        It should_render_the_view_with_the_correct_id = () => 
            _result.AssertViewRendered().ForView("").WithViewData<DeleteRetreatViewModel>().Id.ShouldEqual(100);

        It should_render_the_view_with_the_correct_date = () =>
            _result.AssertViewRendered().ForView("").WithViewData<DeleteRetreatViewModel>().StartDate.ShouldEqual(DateTime.Parse("1/1/2011"));

        static IRetreatRepository _repo;
        static RetreatController _controller;
        static ViewResult _result;
    }

    [Subject("Deleting A Retreat")]
    public class when_deleting_a_retreat
    {
        Establish context = () =>
        {
            _repo = MockRepository.GenerateStub<IRetreatRepository>();
            _controller = new RetreatController(_repo, null);
        };

        Because of = () =>
            _actionResult = _controller.Delete(100,null);

        It should_delete_the_retreat_from_the_repo = () => _repo.AssertWasCalled(x => x.DeleteById(100));

        It should_redirect_to_the_retreat_index = () =>
            _actionResult.AssertActionRedirect().ToAction<RetreatController>(c => c.Index(null));

        static IRetreatRepository _repo;
        static RetreatController _controller;
        static ActionResult _actionResult;
    }
}