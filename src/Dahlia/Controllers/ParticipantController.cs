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
    public class ParticipantController : Controller
    {
        readonly IRetreatRepository _retreatRepository;
        readonly IParticipantRepository _participantRepository;
        readonly IBedRepository _bedRepository;
        readonly IUrlMapper _urlMapper;

        public ParticipantController(IRetreatRepository retreatRepository, IParticipantRepository participantRepository, IBedRepository bedRepository, IUrlMapper urlMapper)
        {
            _retreatRepository = retreatRepository;
            _participantRepository = participantRepository;
            _bedRepository = bedRepository;
            _urlMapper = urlMapper;
        }

        public ViewResult AddToRetreat(int retreatId)
        {
            var retreat = _retreatRepository.GetById(retreatId);
            var beds = _bedRepository.GetAll();

            var viewModel = new AddParticipantToRetreatViewModel
            {
                RetreatId = retreatId,
                RetreatDate = retreat.StartDate,
                DateReceived = DateTime.Today,
                RetreatIsFull = retreat.IsFull,
                Beds = beds,
            };

            return View("AddToRetreat", viewModel);
        }

        [HttpPost]
        public ActionResult AddToRetreat(AddParticipantToRetreatViewModel postBack)
        {
            if (postBack.Cancel != null)
                return this.RedirectToAction<RetreatController>(c => c.Index(postBack.RetreatId));

            if (postBack.Search != null)
            {
                var queryResults = _participantRepository.WithNameLike(postBack.FirstName, postBack.LastName);
                var searchResults = queryResults.Select(x => new ParticipantSearchResultViewModel
                {
                    Name = string.Format("{0} {1}", x.FirstName, x.LastName),
                    DateReceived = x.DateReceived,
                    SelectLink = _urlMapper.MapAction<ParticipantController>(c => c.AssignToRetreatChooseBedCode(postBack.RetreatId, x.Id)),
                });
                TempData["searchResults"] = new AddParticipantToRetreatSearchResultsViewModel
                {
                    SearchResults = searchResults.ToList(),
                };
                return this.RedirectToAction(c => c.AddToRetreat(postBack.RetreatId));
            }

            var retreat = _retreatRepository.Get(postBack.RetreatDate);

            // TODO: what if we already have a participant with this name in the db?
            var newParticipant = new Participant
            {
                FirstName = postBack.FirstName,
                LastName = postBack.LastName,
                DateReceived = postBack.DateReceived,
                Notes = postBack.Notes,
                PhysicalStatus = postBack.PhysicalStatus
            };

            var bed = _bedRepository.GetBy(postBack.BedCode);

            retreat.AddParticipant(newParticipant, bed);
            _retreatRepository.Save(retreat);

            return this.RedirectToAction<RetreatController>(c => c.Index(postBack.RetreatId));
        }

        public ViewResult DeleteFromRetreat(int retreatId, DateTime retreatDate, int participantId, string firstName, string lastName)
        {
            var viewModel = new DeleteParticipantFromRetreatViewModel
                            {
                                RetreatId = retreatId,
                                RetreatDate = retreatDate,
                                ParticipantId = participantId,
                                FirstName = firstName,
                                LastName = lastName,
                            };
            return View("DeleteFromRetreat", viewModel);
        }

        [HttpPost]
        public ActionResult DeleteFromRetreat(DeleteParticipantFromRetreatViewModel viewModel)
        {
            var retreat = _retreatRepository.GetById(viewModel.RetreatId);
            retreat.RemoveParticipant(viewModel.ParticipantId);
            _retreatRepository.Save(retreat);

            return this.RedirectToAction<RetreatController>(c => c.Index(viewModel.RetreatId));
        }

        public ActionResult AssignToRetreatChooseBedCode(int retreatId, int participantId)
        {
            var bedCodes = _bedRepository.GetAll().Select(x => x.Code).OrderBy(x => x);
            var model = new AssignParticipantToRetreatChooseBedCodeViewModel
            {
                RetreatId = retreatId,
                ParticipantId = participantId,
                BedCodeList = new[] {"(none)"}.Concat(bedCodes).ToArray(),
                BedCode = "(none)",
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult AssignToRetreatChooseBedCode(AssignParticipantToRetreatChooseBedCodeViewModel postBack)
        {
            var retreat = _retreatRepository.GetById(postBack.RetreatId);
            var participant = _participantRepository.GetById(postBack.ParticipantId);
            Bed bed = null;
            if (!string.IsNullOrEmpty(postBack.BedCode))
                bed = _bedRepository.GetBy(postBack.BedCode);

            // TODO: this 
            retreat.AddParticipant(participant, bed);
            _retreatRepository.Save(retreat);

            // TODO: do we need to remove the participant from any retreat they were previously
            // registered for?  What if they were registered for multiple retreats?  Do we
            // support that?

            return this.RedirectToAction<RetreatController>(c => c.Index(retreat.Id)); 
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
            ParticipantNote = participant.Notes
          };
          return View(viewModel);
        }
    }
}
