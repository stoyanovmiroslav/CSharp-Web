using System;
using System.Reflection;
using SIS.Framework;
using SIS.Framework.Routers;
using SIS.HTTP.Enums;
using SIS.WebServer;
using SIS.WebServer.Routing;

namespace CakeApp
{
    public class Launcher
    {
        static void Main(string[] args)
        {
            Server server = new Server(80, new ControllerRouter());

            MvcEngine.Run(server);
        }
    }
}