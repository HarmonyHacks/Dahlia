using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;

namespace Dahlia.Specifications.Services
{
    public class when_doing_something : ReportGeneratorTests
    {
        Establish context = () =>
        {
            
        };

        It should_do_something = () =>
            foo.ShouldEqual("bar");
    }

    public class ReportGeneratorTests
    {
        Establish context = () =>
        {
            foo = "bar";
        };

        public static string foo;
    }
}
