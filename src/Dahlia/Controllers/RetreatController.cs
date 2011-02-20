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
    public class RetreatController : Controller
    {
        readonly IRetreatRepository _retreatRepository;
        readonly IUrlMapper _urlMapper;

        public RetreatController(IRetreatRepository retreatRepository, IUrlMapper urlMapper)
        {
            _retreatRepository = retreatRepository;
            _urlMapper = urlMapper;
        }

        public ActionResult Index(string id)
        {
            var model = GetModel();
            model.CurrentRetreatId = id ?? string.Empty;
            return View(model);
        }

        RetreatListViewModel GetModel()
        {
            var retreats = GetRetreats().ToArray();
            
            if (retreats.Count() != 0)
                retreats.First().Active = true;

            return new RetreatListViewModel
                   {
                       CreateLink = _urlMapper.MapAction<RetreatController>(c => c.Create()),
                       Retreats = retreats
                   };
        }

        IEnumerable<RetreatListRetreatViewModel> GetRetreats()
        {
            return _retreatRepository.GetList().OrderBy(x => x.StartDate).Select(
                x => new RetreatListRetreatViewModel
                     {
                         ActualId = x.Id,
                         Date = x.StartDate,
                         AddParticipantLink = AddParticipantLinkForRetreat(x),
                         RegisteredParticipants = x.Registrations.Select(
                             y => new RetreatListParticipantViewModel
                                  {
                                      FirstName = y.Participant.FirstName,
                                      LastName = y.Participant.LastName,
                                      BedCode = y.Bed == null ? null :y.Bed.Code,
                                      DateReceived = y.Participant.DateReceived,
                                      PhysicalStatus = y.Participant.PhysicalStatus,
                                      Notes = y.Participant.Notes,
                                      DeleteLink = BuildDeleteLink(x, y.Participant)
                                  })
                     });
        }

        Uri BuildDeleteLink(Retreat retreat, Participant participant)
        {
            return _urlMapper.MapAction<ParticipantController>(
                x => x.DeleteFromRetreat(
                    retreat.StartDate, 
                    participant.FirstName,
                    participant.LastName));
        }

        Uri AddParticipantLinkForRetreat(Retreat retreat)
        {
            return _urlMapper.MapAction<ParticipantController>(
                x => x.AddToRetreat(retreat.StartDate));
        }

        public ActionResult Create()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(Retreat retreatModel)
        {
            if (!ModelState.IsValid)
                return View();
            _retreatRepository.Add(retreatModel);
            return RedirectToAction("Index", new { id = RetreatUiHelpers.RetreatUiId(retreatModel.StartDate) });
        }

        public ActionResult Delete(int id)
        {
            var retreat = _retreatRepository.GetById(id);
            return View(new DeleteRetreatViewModel {Id = retreat.Id, Description = retreat.Description, StartDate = retreat.StartDate});
        }

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            _retreatRepository.DeleteById(id);
            // TODO: this is more explicit than it needs to be because we can't get MvcContrib.TestHelper to
            // assert the right thing without a controller name for some reason.  Please fix!
            return RedirectToAction("Index", "Retreat");
        }
    }
}
