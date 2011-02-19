using System;
using System.Web.Mvc;
using Dahlia.ViewModels;

namespace Dahlia.Controllers
{
    public class ParticipantController : Controller
    {
        public ViewResult AddToRetreat(DateTime retreatDate)
        {
            var viewModel = new AddParticipantToRetreatViewModel
                                {
                                    RetreatDate = retreatDate,
                                    DateReceived = DateTime.Today,
                                };
            return View("AddToRetreat", viewModel);
        }

    }
}
