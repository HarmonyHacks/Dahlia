using System;
using System.Linq;
using System.Web.Mvc;
using Dahlia.Models;
using Dahlia.Repositories;
using Dahlia.ViewModels;

namespace Dahlia.Controllers
{
    public class ParticipantController : Controller
    {
        readonly IRetreatRepository _retreatRepository;

        public ParticipantController(IRetreatRepository retreatRepository)
        {
            _retreatRepository = retreatRepository;
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

        public ActionResult DoDeleteFromRetreat(DeleteParticipantFromRetreatViewModel viewModel)
        {
            var retreat = _retreatRepository.Get(viewModel.RetreatDate);
            var participantToRemove =
            retreat.Registrations.First(
                    x => x.Participant.FirstName == viewModel.FirstName 
                                && x.Participant.LastName == viewModel.LastName);
            retreat.Registrations.Remove(participantToRemove);
            return RedirectToAction("Index", "Retreat");
        }

        public ActionResult DoAddToRetreat(AddParticipantToRetreatViewModel postBack)
        {
            if (postBack.Cancel != null)
                return RedirectToAction("Index", "Retreat", new {id = postBack.RetreatUiId});

            if(postBack.Search != null)
            {
                TempData["searchResults"] = new AddParticipantToRetreatSearchResultsViewModel();
                return RedirectToAction("AddToRetreat", "Participant");
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

            retreat.AddParticipant(newParticipant, postBack.BedCode);

            _retreatRepository.Save(retreat);

            return RedirectToAction("Index", "Retreat", new { id = postBack.RetreatUiId });
        }

        public ActionResult ReAssignSearchResults()
        {

            var viewModel = new ReassignParticipantSearchResultsViewModel
            {
               Results = new[]
               {
                   new ReassignParticipantSearchResultViewModel { DateReceived = DateTime.Now, Name = "Bob Dobbs", SelectLink = new Uri("/Participant/DoReassign?participantId=42", UriKind.Relative)},
                   new ReassignParticipantSearchResultViewModel { DateReceived = DateTime.Now, Name = "Bob Smith", SelectLink = new Uri("/Participant/DoReassign?participantId=432", UriKind.Relative)},
                   new ReassignParticipantSearchResultViewModel { DateReceived = DateTime.Now, Name = "Bob Jones", SelectLink = new Uri("/Participant/DoReassign?participantId=424", UriKind.Relative)},
               }
            };

            return View(viewModel);
        }

        public ActionResult DoReAssign(int participantId)
        {
            return View();
        }
    }
}
