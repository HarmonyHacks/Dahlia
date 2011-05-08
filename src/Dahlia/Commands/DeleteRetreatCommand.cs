using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dahlia.Models;
using Dahlia.Repositories;
using Dahlia.ViewModels;

namespace Dahlia.Commands
{
    public class DeleteRetreatCommand : IControllerCommand<DeleteRetreatViewModel>
    {
        readonly IRetreatRepository _retreatRepository;

        public DeleteRetreatCommand(IRetreatRepository retreatRepository)
        {
            _retreatRepository = retreatRepository;
        }

        public bool Execute(DeleteRetreatViewModel viewModel)
        {
            try
            {
                _retreatRepository.DeleteById(viewModel.Id);
            }
            catch (Exception e)
            {
                Exception = e;
                return false;
            }

            return true;
        }

        public Exception Exception { get; private set; }
    }
}