﻿using System;
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
        readonly IParticipantSearchService _participantSearchService;
        readonly IBedRepository _bedRepository;

        public ParticipantController(IRetreatRepository retreatRepository, IParticipantRepository participantRepository, IParticipantSearchService participantSearchService, IBedRepository bedRepository)
        {
            _retreatRepository = retreatRepository;
            _participantRepository = participantRepository;
            _participantSearchService = participantSearchService;
            _bedRepository = bedRepository;
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
            _retreatRepository.Save(retreat);

            return RedirectToAction("Index", "Retreat");
        }

        public ActionResult DoAddToRetreat(AddParticipantToRetreatViewModel postBack)
        {
            if (postBack.Cancel != null)
                return RedirectToAction("Index", "Retreat", new {id = postBack.RetreatUiId});

            if(postBack.Search != null)
            {
                var queryResults = _participantSearchService.SearchParticipants(postBack.FirstName, postBack.LastName);
                var searchResults = queryResults.Select(x => new ParticipantSearchResultViewModel
                {
                    Name = string.Format("{0} {1}", x.FirstName, x.LastName),
                    DateReceived = x.DateReceived,
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

            return RedirectToAction("Index", "Retreat", new { id = postBack.RetreatUiId });
        }

        public ActionResult DoAssignToRetreat(DateTime retreatDate, int participantId)
        {
            // use the imaginary participant repository to read the participant.
            // use the retreat repository to get the retreat.
            var retreat = _retreatRepository.Get(retreatDate);
            var participant = _participantRepository.GetById(participantId);
            throw new NotImplementedException();
            //retreat.AddParticipant(participant, );
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
                   new ParticipantSearchResultViewModel { DateReceived = DateTime.Now, Name = "Bob Dobbs", SelectLink = new Uri("/Participant/DoReassign?participantId=42", UriKind.Relative)},
                   new ParticipantSearchResultViewModel { DateReceived = DateTime.Now, Name = "Bob Smith", SelectLink = new Uri("/Participant/DoReassign?participantId=432", UriKind.Relative)},
                   new ParticipantSearchResultViewModel { DateReceived = DateTime.Now, Name = "Bob Jones", SelectLink = new Uri("/Participant/DoReassign?participantId=424", UriKind.Relative)},
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
