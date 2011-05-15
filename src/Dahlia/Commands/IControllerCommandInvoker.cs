using System;
using System.Web.Mvc;

namespace Dahlia.Commands
{
    public interface IControllerCommandInvoker
    {
        ActionResult Invoke<TInput>(TInput input, Type commandType, Func<ActionResult> successResult, Func<ActionResult> failureResult, ModelStateDictionary modelState);
    }
}