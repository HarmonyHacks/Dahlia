using System;
using System.Web.Mvc;
using Dahlia.Models;
using Dahlia.Repositories;
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
        readonly IParticipantRepository _repository;

        public ParticipantController() : this(null, new ParticipantRepository())
        {
            
        }

        public ParticipantController(IRetreatParticipantAdder retreatParticipantAdder, IParticipantRepository repository)
        {
            _retreatParticipantAdder = retreatParticipantAdder;
            _repository = repository;
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

        public ActionResult List()
        {
            return View(_repository.GetAll());
        }
    }
}
