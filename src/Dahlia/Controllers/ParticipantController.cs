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

        public ViewResult AddToRetreat(DateTime retreatDate)
        {
            var retreat = _retreatRepository.Get(retreatDate);

            var viewModel = new AddParticipantToRetreatViewModel
            {
                RetreatDate = retreatDate,
                DateReceived = DateTime.Today,
                RetreatIsFull = retreat.IsFull,
            };

            return View("AddToRetreat", viewModel);
        }

        public ViewResult DeleteFromRetreat(DateTime retreatDate, string firstName, string lastName)
        {
            var viewModel = new DeleteParticipantFromRetreatViewModel
                            {
                                RetreatDate = retreatDate,
                                FirstName = firstName,
                                LastName = lastName,
                            };
            return View("DeleteFromRetreat", viewModel);
        }

        public ActionResult Reassign(ReassignParticipantSearchResultsViewModel viewModel)
        {
            return View("ReassignParticipant", viewModel);
        }

        [HttpPost]
        public ActionResult DeleteFromRetreat(DeleteParticipantFromRetreatViewModel viewModel)
        {
            var retreat = _retreatRepository.Get(viewModel.RetreatDate);
            var participantToRemove =
            retreat.Registrations.First(
                    x => x.Participant.FirstName == viewModel.FirstName 
                                && x.Participant.LastName == viewModel.LastName);
            retreat.Registrations.Remove(participantToRemove);
            _retreatRepository.Save(retreat);

            return RedirectToAction("Index", "Retreat");
        }

        public ActionResult DoAddToRetreat(AddParticipantToRetreatViewModel postBack)
        {
            if (postBack.Cancel != null)
                return RedirectToAction("Index", "Retreat", new {id = postBack.RetreatId});

            if(postBack.Search != null)
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
                return RedirectToAction("AddToRetreat", "Participant", new{ retreatDate = postBack.RetreatDate });
            }

            var retreat = _retreatRepository.Get(postBack.RetreatDate);

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

            return RedirectToAction("Index", "Retreat", new { id = postBack.RetreatId });
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

        public ActionResult DoAssignToRetreat(AssignParticipantToRetreatChooseBedCodeViewModel postBack)
        {
            var retreat = _retreatRepository.GetById(postBack.RetreatId);
            var participant = _participantRepository.GetById(postBack.ParticipantId);
            Bed bed = null;
            if (!string.IsNullOrEmpty(postBack.BedCode))
                bed = _bedRepository.GetBy(postBack.BedCode);
            retreat.AddParticipant(participant, bed);
            return RedirectToAction("Index", "Retreat", new {id = retreat.Id });
        }

        public ActionResult ReAssignSearchResults(string lastnameISearchedFor)
        {
            var ParticipantResults = _participantRepository.WithLastName(lastnameISearchedFor);
            var ViewModel = ParticipantResults.Select(P => new ParticipantSearchResultViewModel()
                            {
                                DateReceived = P.DateReceived, 
                                Name = P.FirstName + " " + P.LastName, 
                                SelectLink = new Uri("anystring", UriKind.Relative)
                            }).ToList();


            return View(ViewModel);
        }

        public ActionResult DoReAssign(int participantId)
        {
            return View();
        }
    }
}
