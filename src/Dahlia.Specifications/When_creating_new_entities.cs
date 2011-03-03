using System;
using System.Data.SQLite;
using Dahlia.Configuration.Persistence;
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
            new SqliteMigrationRunner((SQLiteConnection)_session.Connection).MigrateUp(null);
            _retreat = new Retreat { StartDate = DateTime.Today };
            var participant = new Participant { FirstName = "Dirk", LastName = "Diggler" };

            var bed = new Bed { Code = "bedCode" };

            _retreat.AddParticipant(participant, bed);
        };

        Because of = () =>
        {
            var id = _session.Save(_retreat);
            _session.Flush();

            _persistedRetreat = _session.Load<Retreat>(id);
        };

        It should_save_the_retreat = () => _persistedRetreat.StartDate.ShouldEqual(DateTime.Today);
        It should_save_the_registration = () => _persistedRetreat.Registrations[0].Bed.Code.ShouldEqual("bedCode");
        It should_save_the_participant = () => _persistedRetreat.Registrations[0].Participant.FirstName.ShouldEqual("Dirk");

        static ISession _session;
        static Retreat _retreat;
        static Retreat _persistedRetreat;
        static SQLiteConnection _connection;
    }


    [Subject("New entities are saved")]
    public class When_adding_a_participant_to_a_retreat_with_a_bedcode
    {

        Establish context = () =>
        {
            _session = SQLiteSessionFactory.CreateSessionFactory().OpenSession();
            new SqliteMigrationRunner((SQLiteConnection)_session.Connection).MigrateUp(null);
            
            var retreat = new Retreat { StartDate = DateTime.Today };
            _retreatId = _session.Save(retreat);

            var participant = new Participant { FirstName = "Dirk", LastName = "Diggler" };
            _participantId = _session.Save(participant);

            _session.Flush();
            _session.Evict(retreat);
            _session.Evict(participant);

        };

        Because of = () =>
        {
            var retreat = _session.Load<Retreat>(_retreatId);
            var participant = _session.Load<Participant>(_participantId);
            var bed = new Bed { Code = "FOO" };

            retreat.AddParticipant(participant, bed);

            _session.SaveOrUpdate(retreat);
            _session.Flush();


            _session.Evict(retreat);
            _session.Evict(participant);

           _persistedRetreat =  _session.Load<Retreat>(_retreatId);
        };

        It should_have_a_registration = () =>
            _persistedRetreat.Registrations.Count.ShouldEqual(1);

        It should_have_the_correct_participant_registered = () =>
            _persistedRetreat.Registrations[0].Participant.Id.ShouldEqual(_participantId);

        It should_have_the_correct_bed_code = () =>
            _persistedRetreat.Registrations[0].Bed.Code.ShouldEqual("FOO");
			

        static object _participantId;
        static object _retreatId;
        static ISession _session;
        static Retreat _persistedRetreat;
    }
}