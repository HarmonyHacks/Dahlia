using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Dahlia.Controllers;
using Dahlia.Models;
using Dahlia.Repositories;
using Dahlia.ViewModels;
using Machine.Specifications;
using Rhino.Mocks;

namespace Dahlia.Specifications
{
    [Subject("Searching For Participants")]
    public class when_entering_a_participants_lastname
    {
        Establish context = () =>
        {
            var CurrentSession = SQLiteSessionFactory.CreateSessionFactory().OpenSession();
            Participants = new ParticipantRepository(CurrentSession);

            var AllParticipants = new List<Participant>()
                              {
                                  new Participant() {FirstName = "John", LastName = "Doe"},
                                  new Participant() {FirstName = "Jane", LastName = "Doe"},
                                  new Participant() {FirstName = "Bob", LastName = "Smith"},
                              };

            Participants.Add(AllParticipants);

            ExpectedMatches = AllParticipants.Where(x => x.LastName == "Doe");
        };

        Because of = () =>
        {
            matchingParticipants = Participants.WithLastName("Doe");
        };


        It should_match_all_participants_with_that_lastname = () => matchingParticipants.ShouldContainOnly(ExpectedMatches);


        static IEnumerable<Participant> ExpectedMatches;
        static IEnumerable<Participant> matchingParticipants;
        static IParticipantRepository Participants;
    }

    [Subject("Searching for Participants")]
    public class when_I_search_for_a_participant_by_name
    {
        Establish context = () =>
        {
            participantRepository = MockRepository.GenerateStub<IParticipantRepository>();
            controller = new ParticipantController(null, participantRepository, null, null);
            lastnameISearchedFor = "bob";
            Results = new Participant[] { new Participant(), };
            participantRepository.Stub(x => x.WithLastName(lastnameISearchedFor)).Return(Results);

            controller = new ParticipantController(null, participantRepository, null, null);

            ExpectedResults = Results.Select(x => new ParticipantSearchResultViewModel());
        };

        Because of = () => 
        {
           var result = controller.ReAssignSearchResults(lastnameISearchedFor) as ViewResult;
           SearchResults = result.Model as IEnumerable<ParticipantSearchResultViewModel>;
        };

        It should_search_for_participants_with_my_search_string_in_their_name = () =>
            participantRepository.AssertWasCalled(x => x.WithLastName(lastnameISearchedFor));

        It should_show_me_the_matches_for_what_I_typed_in = () =>
            SearchResults.Count().ShouldEqual(ExpectedResults.Count());
            

        static IParticipantRepository participantRepository;
        static ParticipantController controller;
        static string lastnameISearchedFor;
        static IEnumerable<Participant> Results;
        static IEnumerable<ParticipantSearchResultViewModel> SearchResults;
        static IEnumerable<ParticipantSearchResultViewModel> ExpectedResults;
    }
}