using SIS.Framework;
using SIS.Framework.Routers;
using SIS.WebServer;
using System;

namespace SIS.App
{
    public class StarUp
    {
        static void Main(string[] args)
        {
            Server server = new Server(80, new ControllerRouter());

            MvcEngine.Run(server);
        }
    }
}