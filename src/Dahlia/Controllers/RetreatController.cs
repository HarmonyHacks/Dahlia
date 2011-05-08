using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Dahlia.Commands;
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
        readonly IBedRepository _bedRepository;
        readonly IControllerCommandInvoker _commandInvoker;
        readonly IUrlMapper _urlMapper;
        readonly IReportGeneratorService _reportGenerator;

        public RetreatController(IRetreatRepository retreatRepository, IBedRepository bedRepository, IControllerCommandInvoker commandInvoker, IUrlMapper urlMapper, IReportGeneratorService reportGenerator)
        {
            _retreatRepository = retreatRepository;
            _bedRepository = bedRepository;
            _commandInvoker = commandInvoker;
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
                                  })
                     });
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Retreat viewModel)
        {
            var result = _commandInvoker.Invoke(viewModel,
                                                typeof(CreateRetreatCommand),
                                                () => this.RedirectToAction(c => c.Index(viewModel.Id)),
                                                () => this.RedirectToAction(c => c.Index(viewModel.Id)),
                                                ModelState);
            return result;
        }

        public ActionResult Delete(int id)
        {
            var retreat = _retreatRepository.GetById(id);
            return View(new DeleteRetreatViewModel {Id = retreat.Id, Description = retreat.Description, StartDate = retreat.StartDate});
        }

        [HttpPost]
        public ActionResult Delete(DeleteRetreatViewModel viewModel)
        {
            var result = _commandInvoker.Invoke(viewModel,
                                                typeof(DeleteRetreatCommand),
                                                () => this.RedirectToAction(c => c.Index(null)),
                                                () => this.RedirectToAction(c => c.Index(null)),
                                                ModelState);
            return result;
        }

        public ViewResult AddNewParticipant(int retreatId)
        {
            var retreat = _retreatRepository.GetById(retreatId);
            var beds = _bedRepository.GetAll();

            var viewModel = new AddNewParticipantViewModel
            {
                RetreatId = retreatId,
                RetreatDate = retreat.StartDate,
                RetreatIsFull = retreat.IsFull,
                Beds = retreat.GetUnassignedBeds(beds),
                Participant = new CreateParticipantViewModel
                {
                    DateReceived = DateTime.Today
                },
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult AddNewParticipant(AddNewParticipantViewModel viewModel)
        {
            var result = _commandInvoker.Invoke(viewModel,
                                                typeof(RegisterNewParticipantCommand),
                                                () => this.RedirectToAction(c => c.Index(viewModel.RetreatId)),
                                                () => View(viewModel),
                                                ModelState);
            return result;
        }
        
        public ActionResult UnregisterParticipant(int retreatId, int participantId)
        {
            var retreat = _retreatRepository.GetById(retreatId);
            if (retreat == null)
            {
                return this.RedirectToAction(c => c.Index(retreatId));
            }

            var participant = (from registration in retreat.Registrations
                               where registration.Participant.Id == participantId
                               select registration.Participant).SingleOrDefault();
            if (participant == null)
            {
                return this.RedirectToAction(c => c.Index(retreatId));
            }

            var viewModel = new DeleteParticipantFromRetreatViewModel
            {
                RetreatId = retreatId,
                ParticipantId = participantId,
                RetreatDate = retreat.StartDate,
                FirstName = participant.FirstName,
                LastName = participant.LastName,
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult UnregisterParticipant(DeleteParticipantFromRetreatViewModel viewModel)
        {
            var result = _commandInvoker.Invoke(viewModel,
                                                typeof(UnregisterParticipantCommand),
                                                () => this.RedirectToAction(c => c.Index(viewModel.RetreatId)),
                                                () => this.RedirectToAction(c => c.Index(viewModel.RetreatId)),
                                                ModelState);
            return result;
        }
    }
}
