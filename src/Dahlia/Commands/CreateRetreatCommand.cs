using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dahlia.Models;
using Dahlia.Repositories;

namespace Dahlia.Commands
{
    public class CreateRetreatCommand : IControllerCommand<Retreat>
    {
        readonly IRetreatRepository _retreatRepository;

        public CreateRetreatCommand(IRetreatRepository retreatRepository)
        {
            _retreatRepository = retreatRepository;
        }

        public bool Execute(Retreat viewModel)
        {
            try
            {
                _retreatRepository.Save(viewModel);
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