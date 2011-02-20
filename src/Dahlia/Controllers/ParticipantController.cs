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
            var viewModel = new AddParticipantToRetreatViewModel
                                {
                                    RetreatDate = retreatDate,
                                    DateReceived = DateTime.Today,
                                };
            return View("AddToRetreat", viewModel);
        }

        public ViewResult DeleteFromRetreat(DateTime retreatDate, string firstName, string lastName)
        {
            var viewModel = new DeleteParticipantFromRetreatViewModel()
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
                retreat.RegisteredParticipants.First(
                    x => x.Participant.FirstName == viewModel.FirstName && x.Participant.LastName == viewModel.LastName);
            retreat.RegisteredParticipants.Remove(participantToRemove);
            return RedirectToAction("Index", "Retreat");
        }

        public ActionResult DoAddToRetreat(AddParticipantToRetreatViewModel postBack)
        {
            var retreat = _retreatRepository.Get(postBack.RetreatDate);

            var newParticipant = new Participant
            {
                FirstName = postBack.FirstName,
                LastName = postBack.LastName,
                DateReceived = postBack.DateReceived,
                Notes = postBack.Notes,
            };

            retreat.AddParticipant(newParticipant, postBack.BedCode, postBack.PhysicalStatus);

            _retreatRepository.Save(retreat);

            return RedirectToAction("Index", "Retreat");
        }
    }
}
