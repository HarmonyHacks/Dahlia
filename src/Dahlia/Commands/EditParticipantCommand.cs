using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dahlia.Repositories;
using Dahlia.ViewModels;

namespace Dahlia.Commands
{
    public class EditParticipantCommand : IControllerCommand<EditParticipantViewModel>
    {
        readonly IParticipantRepository _participantRepository;

        public EditParticipantCommand(IParticipantRepository participantRepository)
        {
            _participantRepository = participantRepository;
        }

        public bool Execute(EditParticipantViewModel viewModel)
        {
            var participant = _participantRepository.GetById(viewModel.Id);

            if (participant == null)
            {
                return false;
            }

            participant.FirstName = viewModel.FirstName;
            participant.LastName = viewModel.LastName;
            participant.DateReceived = viewModel.DateReceived;
            participant.Notes = viewModel.Notes;
            participant.PhysicalStatus = viewModel.PhysicalStatus;

            _participantRepository.Save(participant);

            return true;
        }
    }
}