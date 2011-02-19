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
            var model = new RetreatListViewModel
            {
                CreateLink = new Uri("/Retreat/Create", UriKind.Relative),

                Retreats = _retreatRepository.GetList().OrderBy(x => x.StartDate).Select(x => new RetreatListRetreatViewModel
                {
                    Date = x.StartDate,
                    AddParticipantLink = new Uri("../Participant/AddToRetreat?retreatDate=" + x.StartDate.ToString("d"), UriKind.Relative)
                })
            };

            return View(model);
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
