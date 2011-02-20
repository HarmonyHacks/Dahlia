using System;
using Dahlia.Models;
using Machine.Specifications;
using NHibernate;

namespace Dahlia.Specifications
{
    [Subject("New entities are saved")]
    public class When_creating_new_entities
    {
        Establish context = () =>
        {
            _session = SQLiteSessionFactory.CreateSessionFactory().OpenSession();

            _retreat = new Retreat { StartDate = DateTime.Today };
            var participant = new Participant { FirstName = "Dirk", LastName = "Diggler" };

            _retreat.AddParticipant(participant, "bedCode");
        };

        Because of = () =>
        {
            var id = _session.Save(_retreat);
            _session.Flush();

            _persistedRetreat = _session.Load<Retreat>(id);
        };

        It should_save_the_retreat = () => _persistedRetreat.StartDate.ShouldEqual(DateTime.Today);
        It should_save_the_registration = () => _persistedRetreat.Registrations[0].BedCode.ShouldEqual("bedCode");
        It should_save_the_participant = () => _persistedRetreat.Registrations[0].Participant.FirstName.ShouldEqual("Dirk");

        static ISession _session;
        static Retreat _retreat;
        static Retreat _persistedRetreat;
    }
}