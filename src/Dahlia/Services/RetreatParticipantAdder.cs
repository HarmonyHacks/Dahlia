using System;
using Dahlia.Models;

namespace Dahlia.Services
{
    public interface IRetreatParticipantAdder
    {
        void AddParticipantToRetreat(DateTime retreatDate, Participant participant);
    }

    public class RetreatParticipantAdder: IRetreatParticipantAdder
    {
        public void AddParticipantToRetreat(DateTime retreatDate, Participant participant)
        {
            // do nothing so far...
        }
    }
}