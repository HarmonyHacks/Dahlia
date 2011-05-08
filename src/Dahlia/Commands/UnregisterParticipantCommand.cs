using System;
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
            var retreat = _retreatRepository.GetById(viewModel.RetreatId);

            if (retreat == null)
            {
                return false;
            }

            retreat.RemoveParticipant(viewModel.ParticipantId);
            _retreatRepository.Save(retreat);

            return true;
        }
    }
}