using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dahlia.Controllers;
using Dahlia.Models;
using Dahlia.Repositories;
using Dahlia.ViewModels;

namespace Dahlia.Commands
{
    public class AddExistingParticipantToRetreatCommand : IControllerCommand<AddParticipantChooseBedCodeViewModel>
    {
        readonly IRetreatRepository _retreatRepository;
        readonly IParticipantRepository _participantRepository;
        readonly IBedRepository _bedRepository;

        public AddExistingParticipantToRetreatCommand(IRetreatRepository retreatRepository, IParticipantRepository participantRepository, IBedRepository bedRepository)
        {
            _retreatRepository = retreatRepository;
            _participantRepository = participantRepository;
            _bedRepository = bedRepository;
        }

        public bool Execute(AddParticipantChooseBedCodeViewModel viewModel)
        {
            try
            {
                var retreat = _retreatRepository.GetById(viewModel.RetreatId);
                var participant = _participantRepository.GetById(viewModel.ParticipantId);

                Bed bed = null;
                if (!string.IsNullOrEmpty(viewModel.BedCode))
                {
                    bed = _bedRepository.GetBy(viewModel.BedCode);
                }

                retreat.AddParticipant(participant, bed);
                _retreatRepository.Save(retreat);

                return true;
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