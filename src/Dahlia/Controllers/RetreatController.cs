using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Dahlia.Repositories;
using Dahlia.ViewModels;

namespace Dahlia.Controllers
{
    public class RetreatController : Controller
    {
        IRetreatRepository _RetreatRepository;

        public RetreatController()
        {
            _RetreatRepository = new RetreatRepository();
        }
         public ActionResult Index()
         {
             var model = new RetreatListViewModel {Dates = _RetreatRepository.GetList().Select(x => x.StartDate)};

             return View(model);
         }

        public ActionResult Create()
        {
            return View();
        }
    }
}
