using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dahlia.Commands
{
    public interface IControllerCommand<in T>
    {
        bool Execute(T input);
    }
}