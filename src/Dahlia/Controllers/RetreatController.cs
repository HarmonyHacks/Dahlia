using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Dahlia.Models;
using Dahlia.Repositories;
using Dahlia.Services;
using Dahlia.ViewModels;
using MvcContrib;

namespace Dahlia.Controllers
{
    public class RetreatController : Controller
    {
        readonly IRetreatRepository _retreatRepository;
        readonly IUrlMapper _urlMapper;
        readonly IReportGeneratorService _reportGenerator;

        public RetreatController(IRetreatRepository retreatRepository, IUrlMapper urlMapper, IReportGeneratorService reportGenerator)
        {
            _retreatRepository = retreatRepository;
            _urlMapper = urlMapper;
            _reportGenerator = reportGenerator;
        }

        public ActionResult Index(int? id)
        {
            var model = GetModel();
            model.CurrentRetreatId = id ?? -1;
            return View(model);
        }

        public FileResult GenerateReport()
        {
            var result = _reportGenerator.GenerateRetreatsReportExcelHtml();
            return File(result, "application/vnd.ms-excel", "all_retreats_report.xls");
        }

        public FileResult GenerateReportFor(int retreat)
        {
            var result = _reportGenerator.GenerateRetreatReportExcelHtmlFor(retreat);
            return File(result, "application/vnd.ms-excel", "retreat_report_" + retreat.ToString() + ".xls");
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
                         Id = x.Id,
                         Description = x.Description,
                         Date = x.StartDate,
                         AddParticipantLink = AddParticipantLinkForRetreat(x),
                         RegisteredParticipants = x.Registrations.Select(
                             y => new RetreatListParticipantViewModel
                                  {
                                      Id = y.Participant.Id,
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
            // TODO: should have a delete view model here instead?
            return _urlMapper.MapAction<ParticipantController>(
                x => x.DeleteFromRetreat(
                    retreat.Id, 
                    retreat.StartDate,
                    participant.Id,
                    participant.FirstName,
                    participant.LastName));
        }

        Uri AddParticipantLinkForRetreat(Retreat retreat)
        {
            return _urlMapper.MapAction<ParticipantController>(
                x => x.AddToRetreat(retreat.Id));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Retreat retreatModel)
        {
            if (!ModelState.IsValid)
                return View();
            _retreatRepository.Add(retreatModel);
            return this.RedirectToAction(c => c.Index(retreatModel.Id));
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

            return this.RedirectToAction(c => c.Index(null));
        }
    }
}
