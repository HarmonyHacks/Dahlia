using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dahlia.ViewModels
{
    public class DahliaAppViewModel
    {
        public string RetreatsJson { get; set; }
        public RetreatListRetreatViewModel CurrentRetreat { get; set; }
    }
}