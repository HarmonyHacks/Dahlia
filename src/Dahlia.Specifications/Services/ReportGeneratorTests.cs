using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;
using Dahlia.Services;
using Rhino.Mocks;
using Dahlia.Repositories;
using Dahlia.Models;

namespace Dahlia.Specifications.Services
{
    public class when_generating_a_reporting_for_a_Retreat_with_a_participant_who_is_waitlisted : ReportGeneratorTests
    {
        Establish context = () =>
        {
            var retreat = new Retreat();
            var participant = new Participant
            {
                DateReceived = DateTime.Now,
                FirstName = "Johnny",
                LastName = "Tentpeg",
                PhysicalStatus = PhysicalStatus.Unlimited,
                Notes = "something witty",
                Id = 0
            };
            Bed bed = null; // waitlisted Participants have a null bedcode
            retreat.AddParticipant(participant, bed);
            retreatRepo.Expect(rr => rr.GetById(0)).IgnoreArguments().Return(retreat);
            
            error = Catch.Exception(() =>
                reportResult = reportGenerator.GenerateRetreatReportExcelHtmlFor(0)
            );
        };

        It should_not_cause_an_error = () =>
            error.ShouldBeNull();

        It should_return_the_generated_report = () =>
            reportResult.ShouldNotBeNull();
    }

    public class ReportGeneratorTests
    {
        Establish context = () =>
        {
            retreatRepo = MockRepository.GenerateStub<IRetreatRepository>();
            reportGenerator = new ReportGeneratorService(retreatRepo);
        };

        Cleanup after = () =>
        {
            error = null;
            reportResult = null;
        };


        public static IRetreatRepository retreatRepo;
        public static IReportGeneratorService reportGenerator;
        public static IEnumerable<byte> reportResult;
        public static Exception error;
    }
}
