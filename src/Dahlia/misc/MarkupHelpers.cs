using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Dahlia.Models;
using Dahlia.ViewModels;

namespace Dahlia
{
    public static class MarkupHelpers
    {
        public static MvcHtmlString DropDownListFor(
            this HtmlHelper<AddParticipantToRetreatViewModel> htmlHelper,
            Expression<Func<AddParticipantToRetreatViewModel, PhysicalStatus>> expression)
        {
            var stuff =Enum.GetValues(typeof(PhysicalStatus))
                .Cast<PhysicalStatus>()
                .Select(x => new SelectListItem { Text = x.ToString(), Value = x.ToString() });
            return htmlHelper.DropDownListFor(expression, stuff);
        }
    }
}