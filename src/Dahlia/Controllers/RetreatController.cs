using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
<<<<<<< Updated upstream
using Dahlia.Repositories;
using Dahlia.ViewModels;
=======
using Dahlia.Models;
>>>>>>> Stashed changes

namespace Dahlia.Controllers
{
    public class RetreatController : Controller
    {
<<<<<<< Updated upstream
        IRetreatRepository _RetreatRepository;

        public RetreatController()
        {
            _RetreatRepository = new RetreatRepository();
=======
        public ActionResult List()
        {
            var list = new List<Retreat>();

            return View(list);
>>>>>>> Stashed changes
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
