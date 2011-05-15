using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dahlia.ViewModels
{
    public class AddParticipantChooseBedCodeViewModel
    {
        public int RetreatId { get; set; }
        public int ParticipantId { get; set; }
        public string BedCode { get; set; }
        public string[] BedCodeList { get; set; }
        public string Cancel { get; set; }
    }
}