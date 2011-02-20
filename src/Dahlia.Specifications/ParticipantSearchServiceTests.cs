using System;
using System.Collections.Generic;
using System.Linq;
using Dahlia.Controllers;
using Dahlia.Models;
using Dahlia.Repositories;
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
            Participants = new ParticipantRepositoryNHibImpl(CurrentSession);

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
        static ParticipantRepository Participants;
    }

    [Subject("Searching for Participants"), Ignore]
    public class when_I_search_for_a_participant_by_name
    {
        Establish context = () =>
        {
            participantRepository = MockRepository.GenerateStub<ParticipantRepository>();
            controller = new ParticipantController(null, participantRepository, null);
            lastnameISearchedFor = "bob";
        };

        Because of = () =>
            controller.ReAssignSearchResults(lastnameISearchedFor);

        It should_search_for_participants_with_my_search_string_in_their_name = () =>
            participantRepository.AssertWasCalled(x => x.WithLastName(lastnameISearchedFor));

        static ParticipantRepository participantRepository;
        static ParticipantController controller;
        static string lastnameISearchedFor;
    }
}