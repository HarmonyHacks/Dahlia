using System.Web;
using System.Web.Routing;

namespace Dahlia.Services
{
    public interface IRequestContextProvider
    {
        RequestContext RequestContext { get; }
        void SetItem(string key, object value);
        object GetItem(string key);
    }

    public class RequestContextProvider : IRequestContextProvider
    {
        RequestContext IRequestContextProvider.RequestContext
        {
            get { return HttpContext.Current.Request.RequestContext; }
        }

        public void SetItem(string key, object value)
        {
            HttpContext.Current.Items[key] = value;
        }

        public object GetItem(string key)
        {
            return HttpContext.Current.Items[key];
        }
    }
}