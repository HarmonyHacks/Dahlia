using System;
using System.Web.Mvc;
using Dahlia.Controllers;
using Dahlia.Repositories;
using Dahlia.ViewModels;
using Machine.Specifications;
using MvcContrib.TestHelper;
using Rhino.Mocks;

namespace Dahlia.Specifications.Controllers.Retreats
{
    [Subject("Deleting A Retreat")]
    public class when_getting_the_view_to_ask_the_user_about_deleting_a_retreat
    {
        Establish context = () =>
        {
            _repo = MockRepository.GenerateStub<IRetreatRepository>();
            _repo.Stub(x => x.GetById(100)).Return(new Dahlia.Models.Retreat {Id = 100,StartDate = new DateTime(2011,1,1)});
            _controller = new RetreatController(_repo, null, null, null);
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

    [Subject("Deleting a retreat")]
    public class when_the_user_confirms_the_delete : DeleteRetreatContext
    {
        Establish context = () =>
        {
            _viewModel = new DeleteRetreatViewModel { Id = 123 };
            _invoker.ShouldSucceed = true;
        };

        Because of = () =>
            _actionResult = _controller.Delete(_viewModel);

        It should_cause_the_retreat_to_be_deleted = () =>
            _invoker.SuppliedInput.ShouldBeTheSameAs(_viewModel);

        It should_redirect_to_the_retreat_index_view = () =>
            _actionResult.AssertActionRedirect().ToAction<RetreatController>(c => c.Index(null));
    }

    [Subject("Deleting a retreat")]
    public class when_the_user_deletes_an_invalid_retreat : DeleteRetreatContext
    {
        Establish context = () =>
        {
            _viewModel = new DeleteRetreatViewModel { Id = 123 };
            _invoker.ShouldSucceed = false;
        };

        Because of = () =>
            _actionResult = _controller.Delete(_viewModel);

        It should_redirect_to_the_retreat_index_view = () =>
            _actionResult.AssertActionRedirect().ToAction<RetreatController>(c => c.Index(null));
    }

    public class DeleteRetreatContext
    {
        public static FakeControllerCommandInvoker _invoker;
        public static RetreatController _controller;
        public static ViewResult _viewResult;
        public static ActionResult _actionResult;
        public static DeleteRetreatViewModel _viewModel;
        public static IRetreatRepository _retreatRepository;

        Establish context = () =>
        {
            _retreatRepository = MockRepository.GenerateStub<IRetreatRepository>();
            _invoker = new FakeControllerCommandInvoker();
            _controller = new RetreatController(_retreatRepository, null, _invoker, null);
        };
    }
}