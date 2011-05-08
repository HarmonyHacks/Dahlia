using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dahlia.Repositories;
using Dahlia.ViewModels;

namespace Dahlia.Commands
{
    public class RegisterNewParticipantCommand : IControllerCommand<AddNewParticipantViewModel>
    {
        readonly IRetreatRepository _retreatRepository;
        readonly IBedRepository _bedRepository;
        readonly CreateParticipantCommand _createParticipantCommand;

        public RegisterNewParticipantCommand(IRetreatRepository retreatRepository, IBedRepository bedRepository, CreateParticipantCommand createParticipantCommand)
        {
            _retreatRepository = retreatRepository;
            _bedRepository = bedRepository;
            _createParticipantCommand = createParticipantCommand;
        }

        public bool Execute(AddNewParticipantViewModel viewModel)
        {
            try
            {
                if (!_createParticipantCommand.Execute(viewModel.Participant))
                {
                    return false;
                }

                var retreat = _retreatRepository.GetById(viewModel.RetreatId);
                var participant = _createParticipantCommand.CreatedParticipant;
                var bed = _bedRepository.GetBy(viewModel.BedCode);
                
                retreat.AddParticipant(participant, bed);
                
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