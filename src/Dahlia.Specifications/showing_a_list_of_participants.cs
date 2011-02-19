using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Dahlia.Controllers;
using Dahlia.Models;
using Dahlia.Repositories;
using Machine.Specifications;
using Rhino.Mocks;

namespace Dahlia.Specifications
{
    [Subject("showing a list of participants")]
    public class showing_a_list_of_participants
    {
        Establish context =()=>
        {
            _list = new List<Participant>
                    {
                        new Participant {FirstName = "Foo", LastName = "Bar"}
                    };

            _repository = MockRepository.GenerateStub<IParticipantRepository>();
            _repository.Expect(r => r.GetAll()).Return(_list);

            _controller = new ParticipantController(null, _repository);
        };

        Because of =()=>
        {
            _result = _controller.List();
            _model = (List<Participant>)_result.ViewData.Model;
        };

        It should_return_the_participants_from_the_repository =()=>
        {
            _repository.AssertWasCalled(r => r.GetAll());
            _model.SequenceEqual(_list).ShouldEqual(true);
        };

        static List<Participant> _list;
        static IParticipantRepository _repository;
        static ParticipantController _controller;
        static ViewResult _result;
        static List<Participant> _model;
    }
}