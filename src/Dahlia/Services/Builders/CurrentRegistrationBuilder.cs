using System;
using System.Collections.Generic;
using System.Linq;
using Dahlia.Models;
using Dahlia.Repositories;
using Dahlia.ViewModels;

namespace Dahlia.Services.Builders
{
    public class CurrentRegistrationBuilder
    {
        readonly IRetreatRepository _retreatRepository;
        readonly IBedRepository  _bedRepository;

        public CurrentRegistrationBuilder(IRetreatRepository retreatRepository, IBedRepository bedRepository)
        {
            _retreatRepository = retreatRepository;
            _bedRepository = bedRepository;
        }

        public List<CurrentRegistrationViewModel> BuildRegistrationsFor(int participantId)
        {
            var retreats = _retreatRepository.GetForParticipant(participantId);
            var beds = _bedRepository.GetAll();
            
            if(retreats == null ) return new List<CurrentRegistrationViewModel>();

            return retreats.Select(
                retreat => retreat.Registrations
                    .Where(x => x.Participant.Id == participantId)
                    .Select(y => new CurrentRegistrationViewModel
                        {
                                BedCode = y.Bed == null ? "" : y.Bed.Code,
                                Id = y.Id, RetreatId = y.Retreat.Id,
                                RetreatName = y.Retreat.Description,
                                AvailableBedCodes = GetAvailableBedCodes(y.Bed, retreat, beds),
                        }).Single()
                    ).ToList();
        }

        static string[] GetAvailableBedCodes(Bed bed, Retreat retreat, IEnumerable<Bed> beds)
        {
            var bedcodes = retreat.GetUnassignedBeds(beds).Select(x => x.Code).ToArray();
            if (bed != null) bedcodes = new[] { bed.Code }.Concat(bedcodes).ToArray();

            return bedcodes;
        }
    }
}