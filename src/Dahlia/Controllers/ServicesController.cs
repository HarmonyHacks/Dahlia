using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Dahlia.Models;
using Dahlia.Repositories;
using Dahlia.Services;
using Dahlia.ViewModels;

namespace Dahlia.Controllers
{
    public class ServicesController : Controller
    {
        readonly IRetreatRepository _retreatRepository;
        readonly IUrlMapper _urlMapper;
        
        public ServicesController(IRetreatRepository retreatRepository, IUrlMapper urlMapper)
        {
            _retreatRepository = retreatRepository;
            _urlMapper = urlMapper;
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult Retreat()
        {
            var retreats = _retreatRepository.GetList()
                .OrderBy(x => x.StartDate)
                .Select(x => new RetreatListRetreatViewModel
                             {
                                 Date = x.StartDate,
                                 AddParticipantLink = AddParticipantLinkForRetreat(x),
                                 RegisteredParticipants = x.Registrations.Select(y => new RetreatListParticipantViewModel
                                                                {
                                                                    FirstName = y.FirstName,
                                                                    LastName = y.LastName,
                                                                    DateReceived = y.DateReceived,
                                                                    Notes = y.Notes,
                                                                    BedCode = y.BedCode,
                                                                    DeleteLink = DeleteParticipantLinkForRetreat(y.Retreat, y)
                                                                })
                             });

            return Json(retreats);
        }

        Uri AddParticipantLinkForRetreat(Retreat retreat)
        {
            return _urlMapper.MapAction<ParticipantController>(
                x => x.AddToRetreat(retreat.StartDate));
        }

        Uri DeleteParticipantLinkForRetreat(Retreat retreat, Participant participant)
        {
            return _urlMapper.MapAction<ParticipantController>(
                x => x.DeleteFromRetreat(
                    retreat.StartDate,
                    participant.FirstName,
                    participant.LastName));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult Retreat(DateTime date)
        {
            return null; // validation to ensure you don't have more than 1 retreat w/ the same date
        }

        [AcceptVerbs(HttpVerbs.Put)]
        public JsonResult Retreat(Guid id, DateTime date)
        {
            return null;
        }

        [AcceptVerbs(HttpVerbs.Delete)]
        public JsonResult Retreat(string dateOfRetreat)
        {
            DateTime parsedDate = DateTime.Parse(dateOfRetreat);

            return null; //
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult Participant()
        {
            var participants = _retreatRepository.GetList()
                .SelectMany(x => x.Registrations)
                .Select(x => new RetreatListParticipantViewModel
                             {
                                 FirstName = x.FirstName,
                                 LastName = x.LastName,
                                 DateReceived = x.DateReceived,
                                 Notes = x.Notes,
                                 BedCode = x.BedCode,
                                 DeleteLink = DeleteParticipantLinkForRetreat(x.Retreat, x)
                             });

            return Json(participants);
        }
    }
}
