using SIS.Framework.Api;
using SIS.Framework.Routers;
using SIS.Framework.Services;
using SIS.WebServer;
using SIS.WebServer.Api.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.Framework
{
    public class WebHost
    {
        private const int HostingPort = 80;

        public static void Start(IMvcApplication aplication)
        {
            var serviceCollection = new ServiceCollection();
            aplication.ConfigureService(serviceCollection);

            IHttpHandler controllerRouter = new ControllerRouter(serviceCollection);
            IHttpHandler resourceHandler = new ResourceRouter();

            var handlerContext = new HttpRouteHandlingContext(controllerRouter, resourceHandler);

            Server server = new Server(HostingPort, handlerContext);
            server.Run();
        }
    }
}