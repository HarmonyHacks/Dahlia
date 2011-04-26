using System;
using System.Collections.Generic;
using System.Linq;
using Dahlia.Models;

namespace Dahlia.ViewModels
{
    public class ReassignParticipantViewModel
    {
      public string ParticipantName;
      public PhysicalStatus ParticipantPhysicalStatus;
      public string ParticipantNote;
      public IEnumerable<Retreat> AvailableRetreats;
    }
}