using SIS.CakeApp.Controlers;
using SIS.HTTP.Enums;
using SIS.WebServer;
using SIS.WebServer.Routing;
using System;

namespace SIS.CakeApp
{
    public class Launcher
    {
        static void Main(string[] args)
        {
            ServerRoutingTable serverRoutingTable = new ServerRoutingTable();

            serverRoutingTable.Routes[HttpRequestMethod.GET]["/"] = request => new HomeController().Index(request);
            serverRoutingTable.Routes[HttpRequestMethod.GET]["/register"] = request => new AccountController().Register(request);
            serverRoutingTable.Routes[HttpRequestMethod.POST]["/register"] = request => new AccountController().DoRegister(request);
            serverRoutingTable.Routes[HttpRequestMethod.GET]["/login"] = request => new AccountController().Login(request);
            serverRoutingTable.Routes[HttpRequestMethod.POST]["/login"] = request => new AccountController().DoLogin(request);
            serverRoutingTable.Routes[HttpRequestMethod.GET]["/hello"] = request => new HomeController().HelloUser(request);
            serverRoutingTable.Routes[HttpRequestMethod.GET]["/logout"] = request => new AccountController().Logout(request);
            serverRoutingTable.Routes[HttpRequestMethod.GET]["/profile"] = request => new HomeController().Profile(request);
            serverRoutingTable.Routes[HttpRequestMethod.GET]["/addcake"] = request => new HomeController().AddCake(request);
            serverRoutingTable.Routes[HttpRequestMethod.POST]["/addcake"] = request => new HomeController().DoAddCake(request);
            serverRoutingTable.Routes[HttpRequestMethod.GET]["/search"] = request => new HomeController().Search(request);
            serverRoutingTable.Routes[HttpRequestMethod.POST]["/search"] = request => new HomeController().DoSearch(request);
            serverRoutingTable.Routes[HttpRequestMethod.GET]["/aboutus"] = request => new HomeController().AboutUs(request);

            Server server = new Server(80, serverRoutingTable);

            server.Run();
        }
    }
}