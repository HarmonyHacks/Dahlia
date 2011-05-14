using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Dahlia.Commands;
using Machine.Specifications;

namespace Dahlia.Specifications
{
    public class FakeControllerCommandInvoker : IControllerCommandInvoker
    {
        public bool ShouldSucceed { get; set; }
        public object SuppliedInput { get; private set; }
        public Type SuppliedCommandType { get; private set; }

        public ActionResult Invoke<TInput>(TInput input, Type commandType, Func<ActionResult> successResult, Func<ActionResult> failureResult, ModelStateDictionary modelState)
        {
            typeof(IControllerCommand<TInput>).IsAssignableFrom(commandType).ShouldBeTrue();

            SuppliedInput = input;
            SuppliedCommandType = commandType;

            if (ShouldSucceed)
            {
                return successResult();
            }
            else
            {
                return failureResult();
            }
        }
    }
}
