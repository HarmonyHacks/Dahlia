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
    public class ParticipantController : Controller
    {
        readonly IRetreatRepository _retreatRepository;
        readonly IParticipantRepository _participantRepository;
        readonly IBedRepository _bedRepository;
        readonly IUrlMapper _urlMapper;
        readonly IControllerCommandInvoker _commandInvoker;

        public ParticipantController(IRetreatRepository retreatRepository, IParticipantRepository participantRepository, IBedRepository bedRepository, IUrlMapper urlMapper, IControllerCommandInvoker commandInvoker)
        {
            _retreatRepository = retreatRepository;
            _participantRepository = participantRepository;
            _bedRepository = bedRepository;
            _urlMapper = urlMapper;
            _commandInvoker = commandInvoker;
        }

        public ActionResult ReassignSearchResults(string searchString)
        {
            var participantResults = _participantRepository.WithLastName(searchString);
            var viewModel = participantResults.Select(p => new ParticipantSearchResultViewModel()
                            {
                              DateReceived = p.DateReceived,
                              Name = p.FirstName + " " + p.LastName,
                              //SelectLink = new Uri("/Participant/ReassignParticipant?participantId=" + p.Id, UriKind.Relative)
                            }).ToList();


            return View(viewModel);
        }

        public ActionResult Reassign(ReassignParticipantSearchResultsViewModel viewModel)
        {
            return View("ReassignParticipant", viewModel);
        }

        [HttpPost]
        public ActionResult Reassign(int participantId)
        {
            return View();
        }

        public ActionResult ReassignParticipant(int participantId)
        {
          var participant = _participantRepository.GetById(participantId);

          var viewModel = new ReassignParticipantViewModel
          {
            ParticipantName = participant.LastName + ", " + participant.FirstName,
            ParticipantPhysicalStatus = participant.PhysicalStatus,
            ParticipantNote = participant.Notes,
            AvailableRetreats = _retreatRepository.GetList().Where(retreat=>!retreat.IsFull).ToArray(),
          };

          return View(viewModel);
        }

        public ActionResult Edit(int id)
        {
            var participant = _participantRepository.GetById(id);

            if (participant == null)
            {
                return this.RedirectToAction<RetreatController>(c => c.Index(null));
            }

            var participantViewModel = new EditParticipantViewModel
            {
                Id = participant.Id,
                FirstName = participant.FirstName,
                LastName = participant.LastName,
                DateReceived = participant.DateReceived,
                Notes = participant.Notes,
                PhysicalStatus = participant.PhysicalStatus
            };

            return View(participantViewModel);
        }

        [HttpPost]
        public ActionResult Edit(EditParticipantViewModel viewModel)
        {
            var result = _commandInvoker.Invoke(viewModel,
                                                typeof (EditParticipantCommand),
                                                () => this.RedirectToAction<RetreatController>(c => c.Index(null)),
                                                () => View(viewModel), ModelState);
            return result;
        }
    }
}
