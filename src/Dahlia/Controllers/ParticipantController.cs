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
                              SelectLink = new Uri("/Participant/ReassignParticipant?participantId=" + p.Id, UriKind.Relative)
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

            participantViewModel.CurrentRegistrations = GetCurrentRegistrations(participant.Id);

            return View(participantViewModel);
        }

        [HttpPost]
        public ActionResult Edit(EditParticipantViewModel viewModel)
        {
            var result = _commandInvoker.Invoke(viewModel,
                                                typeof (EditParticipantCommand),
                                                () => this.RedirectToAction<RetreatController>(c => c.Index(null)),
                                                () => RedirectToAction("Edit",new {id = viewModel.Id}), ModelState);
            return result;
        }

        List<CurrentRegistrationViewModel> GetCurrentRegistrations(int participantId)
        {
            var results = new List<CurrentRegistrationViewModel>();
            var retreats = _retreatRepository.GetForParticipant(participantId);
            var beds = _bedRepository.GetAll();

            foreach (var retreat in retreats)
            {
                var registration = retreat.Registrations
                    .Where(x => x.Participant.Id == participantId)
                    .Select(y => new CurrentRegistrationViewModel
                        {
                            BedCode = y.Bed == null ? "" : y.Bed.Code,
                            Id = y.Id,
                            RetreatId = y.Retreat.Id,
                            RetreatName = y.Retreat.Description,
                            AvailableBedCodes = GetAvailableBedCodes(y.Bed, retreat, beds),
                        }).Single();
                results.Add(registration);
            }

            return results;
        }

        string[] GetAvailableBedCodes(Bed bed, Retreat retreat, IEnumerable<Bed> beds)
        {
            var bedcodes = retreat.GetUnassignedBeds(beds).Select(x => x.Code).ToArray();

            if (bed != null) bedcodes = new[] {bed.Code}.Concat(bedcodes).ToArray();

            return bedcodes;
        }
    }
}
