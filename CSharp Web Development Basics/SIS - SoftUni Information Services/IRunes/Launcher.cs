using IRunes.Controlers;
using SIS.HTTP.Enums;
using SIS.WebServer;
using SIS.WebServer.Routing;
using System;

namespace IRunes
{
    public class Launcher
    {
        static void Main(string[] args)
        {
            ServerRoutingTable serverRoutingTable = new ServerRoutingTable();

            serverRoutingTable.Routes[HttpRequestMethod.GET]["/"] = request => new HomeController().Index(request);
            serverRoutingTable.Routes[HttpRequestMethod.GET]["/home/index"] = request => new HomeController().Index(request);
            serverRoutingTable.Routes[HttpRequestMethod.GET]["/user/login"] = request => new UserController().Login(request);
            serverRoutingTable.Routes[HttpRequestMethod.POST]["/user/login"] = request => new UserController().LoginPost(request);
            serverRoutingTable.Routes[HttpRequestMethod.GET]["/user/register"] = request => new UserController().Register(request);
            serverRoutingTable.Routes[HttpRequestMethod.POST]["/user/register"] = request => new UserController().RegisterPost(request);
            serverRoutingTable.Routes[HttpRequestMethod.GET]["/user/logout"] = request => new UserController().Logout(request);
            serverRoutingTable.Routes[HttpRequestMethod.GET]["/album/all"] = request => new AlbumController().All(request);
            serverRoutingTable.Routes[HttpRequestMethod.GET]["/album/create"] = request => new AlbumController().Create(request);
            serverRoutingTable.Routes[HttpRequestMethod.POST]["/album/create"] = request => new AlbumController().CreatePost(request);
            serverRoutingTable.Routes[HttpRequestMethod.GET]["/album/details"] = request => new AlbumController().Details(request);
            serverRoutingTable.Routes[HttpRequestMethod.GET]["/track/create"] = request => new TrackController().Create(request);
            serverRoutingTable.Routes[HttpRequestMethod.POST]["/track/create"] = request => new TrackController().CreatePost(request);
            serverRoutingTable.Routes[HttpRequestMethod.GET]["/track/details"] = request => new TrackController().Details(request);

            Server server = new Server(80, serverRoutingTable);

            server.Run();
        }
    }
}