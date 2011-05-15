using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Machine.Specifications;
using Rhino.Mocks;
using StructureMap;
using Dahlia.Commands;

namespace Dahlia.Specifications.Commands
{
    [Subject(typeof(ControllerCommandInvoker))]
    public class when_the_invoker_invokes_a_successful_command : ControllerCommandInvokerContext
    {
        Because of = () =>
            _invokeResult = _invoker.Invoke(FakeCommandBehavior.Succeed, typeof(FakeCommand), () => _successResult, () => _failureResult, _modelState);

        It should_invoke_the_command = () =>
            _command.Executed.ShouldBeTrue();

        It should_invoke_the_success_function = () =>
            _invokeResult.ShouldBeTheSameAs(_successResult);
    }

    [Subject(typeof(ControllerCommandInvoker))]
    public class when_the_invoker_invokes_a_failing_command : ControllerCommandInvokerContext
    {
        Because of = () =>
            _invokeResult = _invoker.Invoke(FakeCommandBehavior.Fail, typeof(FakeCommand), () => _successResult, () => _failureResult, _modelState);

        It should_invoke_the_command = () =>
            _command.Executed.ShouldBeTrue();

        It should_invoke_the_failure_function = () =>
            _invokeResult.ShouldBeTheSameAs(_failureResult);
    }

    [Subject(typeof(ControllerCommandInvoker))]
    public class when_the_invoker_invokes_a_command_with_bad_model_state : ControllerCommandInvokerContext
    {
        Establish context = () =>
            _modelState.AddModelError("key", "message");

        Because of = () =>
            _invokeResult = _invoker.Invoke(FakeCommandBehavior.DoesntMatter, typeof(FakeCommand), () => _successResult, () => _failureResult, _modelState);

        It should_not_invoke_the_command = () =>
            _command.Executed.ShouldBeFalse();

        It should_invoke_the_failure_function = () =>
            _invokeResult.ShouldBeTheSameAs(_failureResult);
    }

    [Subject(typeof(ControllerCommandInvoker))]
    public class when_the_invoker_invokes_a_command_that_throws : ControllerCommandInvokerContext
    {
        Because of = () =>
            _invokeResult = _invoker.Invoke(FakeCommandBehavior.Throw, typeof(FakeCommand), () => _successResult, () => _failureResult, _modelState);

        It should_invoke_the_command = () =>
            _command.Executed.ShouldBeTrue();

        It should_invoke_the_failure_function = () =>
            _invokeResult.ShouldBeTheSameAs(_failureResult);
    }

    public class ControllerCommandInvokerContext
    {
        public static FakeCommand _command;
        public static IContainer _container;
        public static ControllerCommandInvoker _invoker;

        public static ModelStateDictionary _modelState;

        public static ActionResult _successResult = new EmptyResult();
        public static ActionResult _failureResult = new EmptyResult();
        public static ActionResult _invokeResult;

        Establish context = () =>
        {
            _command = new FakeCommand();

            _container = MockRepository.GenerateStub<IContainer>();
            _container.Stub(x => x.GetInstance(typeof(FakeCommand))).Return(_command);

            _invoker = new ControllerCommandInvoker(_container);

            _modelState = new ModelStateDictionary();
        };
    }

    public class FakeCommand : IControllerCommand<FakeCommandBehavior>
    {
        public bool Executed { get; set; }

        public bool Execute(FakeCommandBehavior input)
        {
            Executed = true;

            switch (input)
            {
                case FakeCommandBehavior.Throw:
                    throw new Exception();
                
                case FakeCommandBehavior.Fail:
                    return false;
                
                default:
                    return true;
            }
        }

        public Exception Exception { get; private set; }
    }

    public enum FakeCommandBehavior
    {
        Succeed,
        Fail,
        Throw,
        DoesntMatter
    }
}
