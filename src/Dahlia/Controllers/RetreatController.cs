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
        readonly IParticipantRepository _participantRepository;
        readonly IBedRepository _bedRepository;
        readonly IControllerCommandInvoker _commandInvoker;
        readonly IReportGeneratorService _reportGenerator;

        public RetreatController(IRetreatRepository retreatRepository, IParticipantRepository participantRepository, IBedRepository bedRepository, IControllerCommandInvoker commandInvoker, IReportGeneratorService reportGenerator)
        {
            _retreatRepository = retreatRepository;
            _participantRepository = participantRepository;
            _bedRepository = bedRepository;
            _commandInvoker = commandInvoker;
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

        public ViewResult AddParticipant(int retreatId)
        {
            var viewModel = MakeAddParticipantViewModel(retreatId);

            return View(viewModel);
        }

        AddParticipantViewModel MakeAddParticipantViewModel(int retreatId)
        {
            var retreat = _retreatRepository.GetById(retreatId);
            var beds = _bedRepository.GetAll();

            return new AddParticipantViewModel
            {
                RetreatId = retreatId,
                RetreatIsFull = retreat.IsFull,
                Beds = retreat.GetUnassignedBeds(beds),
                Participant = new CreateParticipantViewModel
                {
                    DateReceived = DateTime.Today
                },
            };
        }

        [HttpPost]
        public ActionResult AddParticipant(AddParticipantViewModel viewModel)
        {
            if (viewModel.Cancel != null)
            {
                return DoCancel(viewModel);
            }
            else if (viewModel.Search != null)
            {
                return DoSearch(viewModel);
            }
            else
            {
                return DoAddNew(viewModel);
            }
        }

        ActionResult DoCancel(AddParticipantViewModel viewModel)
        {
            return this.RedirectToAction(c => c.Index(viewModel.RetreatId));
        }

        ActionResult DoSearch(AddParticipantViewModel viewModel)
        {
            var queryResults = _participantRepository.WithNameLike(viewModel.Participant.FirstName, viewModel.Participant.LastName);
            var searchResults = queryResults.Select(x => new ParticipantSearchResultViewModel
            {
                Id = x.Id,
                Name = string.Format("{0} {1}", x.FirstName, x.LastName),
                DateReceived = x.DateReceived,
            });

            var newViewModel = MakeAddParticipantViewModel(viewModel.RetreatId);
            newViewModel.SearchResults = searchResults.ToList();

            return View(newViewModel);
        }

        ActionResult DoAddNew(AddParticipantViewModel viewModel)
        {
            var result = _commandInvoker.Invoke(viewModel,
                                                typeof(RegisterNewParticipantCommand),
                                                () => this.RedirectToAction(c => c.Index(viewModel.RetreatId)),
                                                () => View(viewModel),
                                                ModelState);
            return result;
        }

        public ActionResult RemoveParticipant(int retreatId, int participantId)
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

            var viewModel = new RemoveParticipantFromRetreatViewModel
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
        public ActionResult RemoveParticipant(RemoveParticipantFromRetreatViewModel viewModel)
        {
            var result = _commandInvoker.Invoke(viewModel,
                                                typeof(RemoveParticipantFromRetreatCommand),
                                                () => this.RedirectToAction(c => c.Index(viewModel.RetreatId)),
                                                () => this.RedirectToAction(c => c.Index(viewModel.RetreatId)),
                                                ModelState);
            return result;
        }
    }
}
