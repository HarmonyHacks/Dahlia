using System;
using System.Web.Mvc;

namespace Dahlia.Controllers
{
    public class ParticipantController : Controller
    {
        public ViewResult AddToRetreat(DateTime retreatDate)
        {
            return View("AddToRetreat");
        }

    }
}
