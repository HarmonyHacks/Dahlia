using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Dahlia.Models;
using Dahlia.Repositories;
using Dahlia.ViewModels;

namespace Dahlia.Controllers
{
    public class RetreatController : Controller
    {
        readonly IRetreatRepository _retreatRepository;

        public RetreatController(IRetreatRepository retreatRepository)
        {
            _retreatRepository = retreatRepository;
        }

        public ActionResult Index()
        {
            var retreats = _retreatRepository.GetList().OrderBy(x => x.StartDate).Select(
                x => new RetreatListRetreatViewModel
                     {
                         Date = x.StartDate,
                         AddParticipantLink = AddParticipantLinkForRetreat(x),
                         RegisteredParticipants = x.RegisteredParticipants.Select(
                             y => new RetreatListParticipantViewModel
                                  {
                                      FirstName = y.Participant.FirstName,
                                      LastName = y.Participant.LastName,
                                      BedCode = y.BedCode,
                                      DateReceived = y.Participant.DateReceived,
                                      Notes = y.Participant.Notes,
                                      DeleteLink = new Uri("/Participant/DeleteFromRetreat?retreatDate=" + x.StartDate + "&firstName=" + y.Participant.FirstName + "&lastName=" + y.Participant.LastName, UriKind.Relative)
                                  })
                     });
            var model = new RetreatListViewModel
            {
                CreateLink = new Uri("/Retreat/Create", UriKind.Relative),

                Retreats =
                    retreats
            };

            return View(model);
        }

        Uri AddParticipantLinkForRetreat(Retreat x)
        {
            return new Uri("../Participant/AddToRetreat?retreatDate=" + x.StartDate.ToString("d"), UriKind.Relative);
        }

        public ActionResult Create()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(Retreat retreatModel)
        {
            if (!ModelState.IsValid)
                return View();
            _retreatRepository.Add(retreatModel);
            return RedirectToAction("Index");
        }
    }
}
