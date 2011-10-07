using System;
using System.Linq;
using Dahlia.Models;
using Dahlia.Repositories;
using Dahlia.ViewModels;
using log4net;

namespace Dahlia.Commands
{
    public class EditParticipantCommand : IControllerCommand<EditParticipantViewModel>
    {
        readonly IParticipantRepository _participantRepository;
        readonly IRetreatRepository _retreatRepository;
        readonly IBedRepository _bedRepository;
        public Exception Exception { get; private set; }
        public ILog Log { get; set; }


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
            Log.InfoFormat("requesting retreats for participant with id {0}", participant.Id);
            var retreats = _retreatRepository.GetForParticipant(participant.Id).ToList();
            var beds = _bedRepository.GetAll();

            foreach (var registration in viewModel.CurrentRegistrations)
            {
                var currentRegistration = registration;
                Log.InfoFormat("current Retreat: {0}", currentRegistration.RetreatId);
                var retreat = retreats.Single(x => x.Id == currentRegistration.RetreatId);
                Log.InfoFormat("the current selected retreat has {0} registrations", retreat.Registrations.Count());
                var updateRegistration = retreat.Registrations.Single(x => x.Participant.Id == participant.Id);
                Log.InfoFormat("the current registrations dedcode is {0}", currentRegistration.BedCode);
                Log.InfoFormat("the beds collection has {0} beds ", beds.Count());
                updateRegistration.Bed = beds.Select(b => b.Code).Contains(currentRegistration.BedCode)
                                ? beds.Single(b => b.Code == currentRegistration.BedCode)
                                : null;

               _retreatRepository.Save(retreat);
            }
        }


    }
}