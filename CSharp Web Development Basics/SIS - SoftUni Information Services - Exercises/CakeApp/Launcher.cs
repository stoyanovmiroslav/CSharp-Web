using System;
using CakeApp.Controllers;
using SIS.Framework;
using SIS.Framework.Routers;
using SIS.Framework.Services;
using SIS.Framework.Services.Contracts;
using SIS.HTTP.Enums;
using SIS.WebServer;
using SIS.WebServer.Api.Contracts;

namespace CakeApp
{
    public class Launcher
    {
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddService<IHashService, HashService>();

            IHttpHandler controllerRouter = new ControllerRouter(serviceCollection);
            IHttpHandler resourceHandler = new ResourceRouter();

            var handlerContext = new HttpRouteHandlingContext(controllerRouter, resourceHandler);

            Server server = new Server(80, handlerContext);

            MvcEngine.Run(server);
        }
    }
}