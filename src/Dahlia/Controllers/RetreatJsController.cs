using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Dahlia.Repositories;
using Dahlia.ViewModels;

namespace Dahlia.Controllers
{
    public class RetreatJsController : Controller
    {
        readonly IRetreatRepository _retreatRepository;

        public RetreatJsController(IRetreatRepository retreatRepository)
        {
            _retreatRepository = retreatRepository;
        }

        public JsonResult Index()
        {
            var model = new RetreatListViewModel
            {
                CreateLink = new Uri("/Retreat/Create", UriKind.Relative),

                Retreats = _retreatRepository.GetList().Select(x => new RetreatListRetreatViewModel
                {
                    Date = x.StartDate,
                    AddParticipantLink = new Uri("../Participant/AddToRetreat?retreatDate=" + x.StartDate.ToString("d"), UriKind.Relative)
                })
            };

            return Json(model, "text/text", JsonRequestBehavior.AllowGet);
        }
    }
}
