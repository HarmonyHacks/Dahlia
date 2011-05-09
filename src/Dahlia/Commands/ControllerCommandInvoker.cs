using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StructureMap;

namespace Dahlia.Commands
{
    public class ControllerCommandInvoker : IControllerCommandInvoker
    {
        readonly IContainer _container;

        public ControllerCommandInvoker(IContainer container)
        {
            _container = container;
        }

        public ActionResult Invoke<TInput>(TInput input, Type commandType, Func<ActionResult> successResult, Func<ActionResult> failureResult, ModelStateDictionary modelState)
        {
            var command = GetCommandInstance<TInput>(commandType);

            if (modelState.IsValid)
            {
                return ExecuteCommand(input, command, successResult, failureResult);
            }
            else
            {
                return failureResult();
            }
        }

        IControllerCommand<TInput> GetCommandInstance<TInput>(Type commandType)
        {
            return _container.GetInstance(commandType) as IControllerCommand<TInput>;
        }

        ActionResult ExecuteCommand<TInput>(TInput input, IControllerCommand<TInput> command, Func<ActionResult> successResult, Func<ActionResult> failureResult)
        {
            bool isSuccessful = TryExecuteCommand(input, command);
            
            if (isSuccessful)
            {
                return successResult();
            }
            else
            {
                // TODO: if the command fails we should shove information into the modelstate for display
                return failureResult();
            }
        }

        bool TryExecuteCommand<TInput>(TInput input, IControllerCommand<TInput> command)
        {
            try
            {
                return command.Execute(input);
            }
            // TODO: smaller exception filter
            catch (Exception)
            {
                // TODO: put the exception message into the model state?
                return false;
            }
        }
    }
}