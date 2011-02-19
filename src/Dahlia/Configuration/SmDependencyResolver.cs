using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using StructureMap;

namespace Dahlia.Configuration
{
    /*
    public class SmDependencyResolver : IDependencyResolver {

        private readonly IContainer _container;

        public SmDependencyResolver(IContainer container) {
            _container = container;
        }

        public object GetService(Type serviceType) {
            if (serviceType == null) 
                return null;
            
            if (serviceType.IsAbstract || serviceType.IsInterface)
            {
                var instance = _container.TryGetInstance(serviceType);
                if (instance == null)
                    throw new InvalidOperationException("Could not resolve component for service: " + serviceType.FullName);
                return instance;
            }
            return _container.GetInstance(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType) {
            return _container.GetAllInstances<object>().Where(s => s.GetType() == serviceType);
        }
    }
    */

    public class SmControllerFactory: DefaultControllerFactory
    {
        private readonly IContainer _container;

        public SmControllerFactory(IContainer container)
        {
            _container = container;
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return (IController)_container.GetInstance(controllerType);
        }
    }
}