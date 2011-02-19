using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Dahlia.Models;
using Dahlia.Repositories;
using Dahlia.ViewModels;

namespace Dahlia.Controllers
{
    public class ServicesController : Controller
    {
        readonly IRetreatRepository _retreatRepository;

        public ServicesController(IRetreatRepository retreatRepository)
        {
            _retreatRepository = retreatRepository;
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult Retreat()
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

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult Retreat(DateTime date)
        {
            return null; // validation to ensure you don't have more than 1 retreat w/ the same date
        }

        [AcceptVerbs(HttpVerbs.Put)]
        public JsonResult Retreat(Guid id, DateTime date)
        {
            return null;
        }

        [AcceptVerbs(HttpVerbs.Delete)]
        public JsonResult Retreat(Guid id)
        {
            return null; //
        }



        //public JsonResult Retreat(string dateTimeOfRetreat)
        //{
        //    var parsedDate = DateTime.Parse(dateTimeOfRetreat);
        //    _retreatRepository.Get(parsedDate);
        //    return Json(new RetreatListViewModel());
        //}

        //[AcceptVerbs(HttpVerbs.Put)]
        //public JsonResult Retreat(RetreatListViewModel model )
        //{
        //    Models.Retreat

        //    _retreatRepository.Add();
        //}

    }
}
