using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dahlia.Models;
using Dahlia.Repositories;
using Dahlia.ViewModels;

namespace Dahlia.Commands
{
    public class CreateParticipantCommand : IControllerCommand<CreateParticipantViewModel>
    {
        readonly IParticipantRepository _participantRepository;

        public CreateParticipantCommand(IParticipantRepository participantRepository)
        {
            _participantRepository = participantRepository;
        }

        public bool Execute(CreateParticipantViewModel viewModel)
        {
            try
            {
                var participant = new Participant();
                participant.FirstName = viewModel.FirstName;
                participant.LastName = viewModel.LastName;
                participant.DateReceived = viewModel.DateReceived;
                participant.Notes = viewModel.Notes;
                participant.PhysicalStatus = viewModel.PhysicalStatus;

                _participantRepository.Save(participant);

                CreatedParticipant = participant;
            }
            catch (Exception e)
            {
                Exception = e;
                return false;
            }

            return true;
        }

        public Exception Exception { get; private set; }

        public Participant CreatedParticipant { get; private set; }
    }
}