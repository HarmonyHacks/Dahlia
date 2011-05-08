using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dahlia.Repositories;
using Dahlia.ViewModels;

namespace Dahlia.Commands
{
    public class UnregisterParticipantCommand : IControllerCommand<DeleteParticipantFromRetreatViewModel>
    {
        readonly IRetreatRepository _retreatRepository;

        public UnregisterParticipantCommand(IRetreatRepository retreatRepository)
        {
            _retreatRepository = retreatRepository;
        }

        public bool Execute(DeleteParticipantFromRetreatViewModel viewModel)
        {
            try
            {
                var retreat = _retreatRepository.GetById(viewModel.RetreatId);

                if (retreat == null)
                {
                    return false;
                }

                retreat.RemoveParticipant(viewModel.ParticipantId);
                _retreatRepository.Save(retreat);
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