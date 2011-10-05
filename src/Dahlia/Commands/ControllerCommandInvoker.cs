using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;
using StructureMap;

namespace Dahlia.Commands
{
    public class ControllerCommandInvoker : IControllerCommandInvoker
    {
        readonly IContainer _container;
        readonly ILog _logger;

        public ControllerCommandInvoker(IContainer container)
        {
            _container = container;
            _logger = _container.GetInstance<ILog>();
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
                _logger.ErrorFormat("failed to execute your {0} command : ", commandType);
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
                _logger.ErrorFormat("Something bad happened... this is what I know\r\n message: {0} \r\n stacktrace: {1}", command.Exception.Message, command.Exception.StackTrace);
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
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
