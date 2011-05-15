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
    [Subject("Creating a retreat")]
    public class when_the_user_starts_to_create_a_retreat
    {
        Establish context = () =>
        {
            _repo = MockRepository.GenerateStub<IRetreatRepository>();
            _repo.Stub(x => x.GetById(100)).Return(new Dahlia.Models.Retreat { Id = 100, StartDate = new DateTime(2011, 1, 1) });
            _controller = new RetreatController(_repo, null, null, null, null);
        };

        Because of = () => _result = (ViewResult)_controller.Create();

        It should_render_the_view_with_the_correct_date = () =>
            _result.AssertViewRendered().ForView("");

        static IRetreatRepository _repo;
        static RetreatController _controller;
        static ViewResult _result;
    }

    [Subject("Creating a retreat")]
    public class when_the_user_saves_the_new_retreat
    {
        Establish context = () =>
        {
            _repo = MockRepository.GenerateStub<IRetreatRepository>();
            _viewModel = new Retreat();
            _invoker = new FakeControllerCommandInvoker();
            _controller = new RetreatController(_repo, null, null, _invoker, null);
        };

        Because of = () =>
            _actionResult = _controller.Create(_viewModel);

        It should_cause_the_retreat_to_be_created = () =>
            _invoker.SuppliedInput.ShouldBeTheSameAs(_viewModel);

        It should_redirect_to_the_retreat_index_view = () =>
            _actionResult.AssertActionRedirect().ToAction<RetreatController>(c => c.Index(123));

        static IRetreatRepository _repo;
        static RetreatController _controller;
        static ActionResult _actionResult;
        static Retreat _viewModel;
        public static FakeControllerCommandInvoker _invoker;
    }
}
