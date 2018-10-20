using SIS.HTTP.Common;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contracts;
using SIS.WebServer.Api.Contracts;
using System.Linq;

namespace SIS.Framework.Routers
{
    public class HttpRouteHandlingContext : IHttpHandlingContext
    {
        public HttpRouteHandlingContext(IHttpHandler controllerHandler, IHttpHandler resourceHandler)
        {
            this.ControllerHandler = controllerHandler;
            this.ResourceHandler = resourceHandler;
        }

        protected IHttpHandler ControllerHandler { get; }

        protected IHttpHandler ResourceHandler { get; }

        public IHttpResponse Handle(IHttpRequest request)
        {
            if (IsResource(request))
            {
                return this.ResourceHandler.HandlerRequest(request);
            }

            return this.ControllerHandler.HandlerRequest(request);
        }

        public bool IsResource(IHttpRequest httpRequest)
        {
            var requestPath = httpRequest.Path;
            if (requestPath.Contains('.'))
            {
                var requestPathExtension = requestPath
                    .Substring(requestPath.LastIndexOf('.'));
                return GlobalConstans.RESOURCE_EXTENSIONS.Contains(requestPathExtension);
            }
            return false;
        }
    }
}