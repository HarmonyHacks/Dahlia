using System;
using System.Collections.Generic;
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
        readonly ParticipantRepository _participantRepository;

        public ParticipantController(IRetreatRepository retreatRepository, ParticipantRepository participantRepository)
        {
            _retreatRepository = retreatRepository;
            _participantRepository = participantRepository;
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
                    x => x.FirstName == viewModel.FirstName && x.LastName == viewModel.LastName);
            retreat.Registrations.Remove(participantToRemove);
            return RedirectToAction("Index", "Retreat");
        }

        public ActionResult DoAddToRetreat(AddParticipantToRetreatViewModel postBack)
        {
            if (postBack.Cancel != null)
                return RedirectToAction("Index", "Retreat", new {id = postBack.RetreatUiId});

            var retreat = _retreatRepository.Get(postBack.RetreatDate);

            var newParticipant = new Participant
                                 {
                                     FirstName = postBack.FirstName,
                                     LastName = postBack.LastName,
                                     DateReceived = postBack.DateReceived,
                                     Notes = postBack.Notes,
                                     BedCode = postBack.BedCode,
                                     PhysicalStatus = postBack.PhysicalStatus,
                                     Retreat = retreat,
                                 };

            retreat.AddParticipant(newParticipant);

            _retreatRepository.Save(retreat);

            return RedirectToAction("Index", "Retreat", new { id = postBack.RetreatUiId });
        }

        public ActionResult ReAssignSearchResults(string lastnameISearchedFor)
        {
            //var ParticipantResults = _participantRepository.WithLastName(lastnameISearchedFor);
            //var viewModel = new List<ReassignParticipantSearchResultsViewModel>(); 

            //foreach (var P in ParticipantResults)
            //{
            //    viewModel.Add(new ReassignParticipantSearchResultViewModel { DateReceived = P.DateReceived, Name  = P.FirstName + " " + P.LastName, new Uri()});        
            //}


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
