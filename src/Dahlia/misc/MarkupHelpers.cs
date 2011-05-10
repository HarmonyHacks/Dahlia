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
        public static MvcHtmlString DropDownListForEnumeration<THelper, TEnum>(
            this HtmlHelper<THelper> htmlHelper,
            Expression<Func<THelper, TEnum>> expression)
        {
            var stuff = Enum.GetValues(typeof(TEnum))
                .Cast<TEnum>()
                .Select(x => new SelectListItem { Text = x.ToString(), Value = x.ToString() });
            return htmlHelper.DropDownListFor(expression, stuff);
        }
    }
}