using IRunes.Controlers;
using SIS.HTTP.Enums;
using SIS.MvcFramework;
using SIS.WebServer;
using SIS.WebServer.Routing;
using System;

namespace IRunes
{
    public class Launcher
    {
        static void Main(string[] args)
        {
            WebHost.Start(new StartUp());   
        }
    }
}