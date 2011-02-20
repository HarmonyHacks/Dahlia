using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;
using NHibernate;
using Dahlia.Repositories;

namespace Dahlia.Specifications
{
    class when_using_retreat_repo
    {
        Establish context = () =>
        {
            RetreatRepository repo = new RetreatRepository();
        };

        Because of = () =>
        {
            _session = SQLiteSessionFactory.CreateSessionFactory().OpenSession();
        };

        It can_save_a_retreat = () =>
        {


        };


        private static ISession _session;


    }
}
