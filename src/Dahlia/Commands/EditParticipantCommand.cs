using System;
using System.Linq;
using Dahlia.Models;
using Dahlia.Repositories;
using Dahlia.ViewModels;

namespace Dahlia.Commands
{
    public class EditParticipantCommand : IControllerCommand<EditParticipantViewModel>
    {
        readonly IParticipantRepository _participantRepository;
        readonly IRetreatRepository _retreatRepository;
        readonly IBedRepository _bedRepository;
        public Exception Exception { get; private set; }

        public EditParticipantCommand(IParticipantRepository participantRepository, IRetreatRepository retreatRepository, IBedRepository bedRepository)
        {
            _participantRepository = participantRepository;
            _retreatRepository = retreatRepository;
            _bedRepository = bedRepository;
        }

        public bool Execute(EditParticipantViewModel viewModel)
        {
            try
            {
                var participant = _participantRepository.GetById(viewModel.Id);

                participant.FirstName = viewModel.FirstName;
                participant.LastName = viewModel.LastName;
                participant.DateReceived = viewModel.DateReceived;
                participant.Notes = viewModel.Notes;
                participant.PhysicalStatus = viewModel.PhysicalStatus;
                
                UpdateBedCodes(participant, viewModel);

                _participantRepository.Save(participant);
            }
            catch (Exception e)
            {
                Exception = e;
                return false;
            }

            return true;
        }

        void UpdateBedCodes(Participant participant, EditParticipantViewModel viewModel)
        {
            var retreats = _retreatRepository.GetForParticipant(participant.Id).ToList();
            var beds = _bedRepository.GetAll();

            foreach (var registration in viewModel.CurrentRegistrations)
            {
               var currentRegistration = registration;
               var retreat = retreats.Single(x => x.Id == currentRegistration.RetreatId);
               retreat.Registrations.Single(x => x.Participant.Id == participant.Id).Bed = 
                                                               beds.Select(b => b.Code).Contains(currentRegistration.BedCode) 
                                                               ? beds.Single(b => b.Code == currentRegistration.BedCode) 
                                                               : null;

               _retreatRepository.Save(retreat);
            }
        }


    }
}