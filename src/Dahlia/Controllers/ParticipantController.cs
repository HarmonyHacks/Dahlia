using System;
using System.Web.Mvc;
using Dahlia.Models;
using Dahlia.ViewModels;

namespace Dahlia.Controllers
{
    public interface IRetreatParticipantAdder
    {
        void AddParticipantToRetreat(DateTime retreatDate, Participant participant);
    }

    public class ParticipantController : Controller
    {
        readonly IRetreatParticipantAdder _retreatParticipantAdder;

        public ParticipantController(IRetreatParticipantAdder retreatParticipantAdder)
        {
            _retreatParticipantAdder = retreatParticipantAdder;
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

        public ActionResult DoAddToRetreat(AddParticipantToRetreatViewModel postBack)
        {
            _retreatParticipantAdder.AddParticipantToRetreat(postBack.RetreatDate, null);
            return new EmptyResult();
        }
    }
}
