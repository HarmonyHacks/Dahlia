using System;
using Dahlia.Configuration.Persistence;
using Machine.Specifications;
using FluentNHibernate.Testing;
using Dahlia.Models;

namespace Dahlia.Specifications.Configuration
{
    public class when_checking_mappings
    {
        It can_correctly_map_retreat = () =>
        {
            var session = SQLiteSessionFactory.
                CreateSessionFactory().OpenSession();

            new PersistenceSpecification<Retreat>(session)
                    .CheckProperty(c => c.Id, 1)
                    .CheckProperty(c => c.StartDate, new DateTime(2011,2,12))
                    .CheckProperty(c => c.Description, "Great Day Retreat")
                    .VerifyTheMappings();
        };

        It can_correctly_map_participant = () =>
        {
            var session = SQLiteSessionFactory.
                CreateSessionFactory().OpenSession();

            new PersistenceSpecification<Participant>(session)
                    .CheckProperty(c => c.Id, 1)
                    .CheckProperty(c => c.DateReceived, new DateTime(2011, 2, 12))
                    .CheckProperty(c => c.FirstName, "Mikers")
                    .CheckProperty(c => c.LastName, "Penis")
                    .CheckProperty(c => c.Notes, "Big Penis")
                    .VerifyTheMappings();
           
        };


    }
}
