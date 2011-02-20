using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;

namespace Dahlia.Services
{
    public interface IUrlMapper
    {
        Uri MapAction<TController>(Expression<Func<TController, ActionResult>> controllerAction) where TController : Controller;
    }

    public class UrlMapper : IUrlMapper
    {
        private readonly IRequestContextProvider _requestContextProvider;
        private readonly RouteCollection _routes;

        public UrlMapper(IRequestContextProvider requestContextProvider, RouteCollection routes)
        {
            _requestContextProvider = requestContextProvider;
            _routes = routes;
        }

        public Uri MapAction<TController>(Expression<Func<TController, ActionResult>> controllerAction) 
            where TController : Controller
        {
            var methodName = controllerAction.GetMethodName();
            var valueDictionary = new RouteValueDictionary();
            controllerAction.ForEachParameter((paramInfo, value) => valueDictionary.Add(paramInfo.Name, value));
            var controllerClassName = typeof(TController).Name;
            var controllerName = controllerClassName.Remove(controllerClassName.Length - "Controller".Length);
            var helper = new UrlHelper(_requestContextProvider.RequestContext, _routes);
            return new Uri(helper.Action(methodName, controllerName, valueDictionary), UriKind.Relative);
        }
    }
}