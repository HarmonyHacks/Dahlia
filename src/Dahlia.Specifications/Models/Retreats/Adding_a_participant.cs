using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dahlia.Controllers;
using Dahlia.Models;
using Dahlia.Repositories;
using Machine.Specifications;
using Rhino.Mocks;

namespace Dahlia.Specifications.Models.Retreats
{
    [Subject("Adding a participant")]
    public class when_adding_a_participant_to_a_retreat_that_has_available_beds
    {
        Establish context = () =>
        {
            _bed = new Bed {Code = "bedcode"};
            _participant = new Participant();
            _retreat = new Retreat();
        };

        Because of = () =>
            _retreat.AddParticipant(_participant, _bed);

        It should_add_the_participant_to_the_retreat = () =>
            _retreat.Registrations.Any(r => r.Participant == _participant).ShouldBeTrue();

        static Bed _bed;
        static Participant _participant;
        static Retreat _retreat;
    }

    [Subject("Adding a participant")]
    public class when_adding_a_participant_to_a_full_retreat
    {
        Establish context = () =>
        {
            _bed = new Bed { Code = "bedcode" };
            _participant = new Participant();
            _retreat = new Retreat();

            FillRetreat(_retreat);
        };

        Because of = () =>
            _exception = Catch.Exception(() => _retreat.AddParticipant(_participant, _bed));

        It should_not_be_allowed = () =>
            _exception.ShouldNotBeNull();

        static void FillRetreat(Retreat retreat)
        {
            for (int x = 0; x < 29; x++)
            {
                var bed = new Bed { Code = "bedcode" + x };
                var participant = new Participant();

                retreat.AddParticipant(participant, bed);
            }
        }

        static Bed _bed;
        static Participant _participant;
        static Retreat _retreat;
        static Exception _exception;
    }

    [Subject("Adding a participant")]
    public class when_adding_a_participant_with_a_bed_that_has_already_been_assigned
    {
        Establish context = () =>
        {
            _bed = new Bed { Code = "bedcode" };
            _participant = new Participant();
            _retreat = new Retreat();

            _retreat.AddParticipant(_participant, _bed);
        };

        Because of = () =>
            _exception = Catch.Exception(() => _retreat.AddParticipant(_participant, _bed));

        It should_not_be_allowed = () =>
            _exception.ShouldNotBeNull();

        static Bed _bed;
        static Participant _participant;
        static Retreat _retreat;
        static Exception _exception;
    }

    [Subject("Adding a participant")]
    public class when_adding_a_participant_to_the_waitlist
    {
        Establish context = () =>
        {
            _participant = new Participant();
            _retreat = new Retreat();
        };

        Because of = () =>
            _retreat.AddParticipant(_participant, null);

        It should_add_the_participant_to_the_waitlist = () =>
            _retreat.Registrations.Any(r => r.Participant == _participant && r.Bed == null).ShouldBeTrue();

        static Participant _participant;
        static Retreat _retreat;
    }

    [Subject("Adding a participant")]
    public class when_adding_multiple_participants_to_the_waitlist
    {
        Establish context = () =>
        {
            _participant = new Participant();
            _retreat = new Retreat();

            _retreat.AddParticipant(new Participant(), null);
        };

        Because of = () =>
            _retreat.AddParticipant(_participant, null);

        It should_add_the_participant_to_the_waitlist = () =>
            _retreat.Registrations.Any(r => r.Participant == _participant && r.Bed == null).ShouldBeTrue();

        static Participant _participant;
        static Retreat _retreat;
    }
}
