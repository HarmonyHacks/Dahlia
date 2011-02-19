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
            var retreats = _retreatRepository.GetList().OrderBy(x => x.StartDate).Select(
               x => new RetreatListRetreatViewModel
               {
                   Date = x.StartDate,
                   AddParticipantLink = AddParticipantLinkForRetreat(x),
                   RegisteredParticipants = x.RegisteredParticipants.Select(
                       y => new RetreatListParticipantViewModel
                       {
                           FirstName = y.Participant.FirstName,
                           LastName = y.Participant.LastName,
                           BedCode = y.BedCode,
                           DateReceived = y.Participant.DateReceived,
                           Notes = y.Participant.Notes,
                           DeleteLink = BuildDeleteLink(x, y)
                       })
               });
            var model = new RetreatListViewModel
            {
                CreateLink = new Uri("/Retreat/Create", UriKind.Relative),

                Retreats = retreats
            };

            return Json(model);  //"text/text", JsonRequestBehavior.AllowGet);
        }

        Uri BuildDeleteLink(Retreat retreat, RegisteredParticipant participant)
        {
            return _urlMapper.MapAction<ParticipantController>(
                x => x.DeleteFromRetreat(
                    retreat.StartDate,
                    participant.Participant.FirstName,
                    participant.Participant.LastName));
        }

        Uri AddParticipantLinkForRetreat(Retreat retreat)
        {
            return _urlMapper.MapAction<ParticipantController>(
                x => x.AddToRetreat(retreat.StartDate));
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



        //public JsonResult Retreat(string dateTimeOfRetreat)
        //{
        //    var parsedDate = DateTime.Parse(dateTimeOfRetreat);
        //    _retreatRepository.Get(parsedDate);
        //    return Json(new RetreatListViewModel());
        //}

        //[AcceptVerbs(HttpVerbs.Put)]
        //public JsonResult Retreat(RetreatListViewModel model )
        //{
        //    Models.Retreat

        //    _retreatRepository.Add();
        //}

    }
}
