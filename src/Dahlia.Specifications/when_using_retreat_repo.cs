using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;
using NHibernate;
using Dahlia.Repositories;
using Dahlia.Persistence;
using Dahlia.Models;

namespace Dahlia.Specifications
{
    class when_using_retreat_repo
    {
        static ISession _session;
        static Retreat _retreat;
        static RetreatRepository _repo;

        Establish context = () =>
        {
            _session = SQLSessionFactory.CreateSessionFactory().OpenSession();
            _repo = new RetreatRepository(_session);
            _retreat = new Retreat()
            {
                Description = "Mike",
                StartDate = DateTime.Now,
                RegisteredParticipants = new List<RegisteredParticipant>(),
            };
        };

        It can_save_a_retreat = () =>
        {
            _repo.Save(_retreat);        
        };

    }
}
