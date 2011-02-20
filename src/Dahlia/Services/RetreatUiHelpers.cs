using System;
using System.Collections.Generic;
using System.Linq;

namespace Dahlia.Services
{
    public static class RetreatUiHelpers
    {
        public static string RetreatUiId(DateTime retreatDate)
        {
            return retreatDate.ToShortDateString().Replace("/", "_");
        }
    }
}