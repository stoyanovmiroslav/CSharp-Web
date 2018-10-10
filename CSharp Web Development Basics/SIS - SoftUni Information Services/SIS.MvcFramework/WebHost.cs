using SIS.HTTP.Enums;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contracts;
using SIS.MvcFramework.Contracts;
using SIS.MvcFramework.HttpAttributes;
using SIS.WebServer;
using SIS.WebServer.Results;
using SIS.WebServer.Routing;
using System;
using System.Linq;
using System.Reflection;

namespace SIS.MvcFramework
{
    public static class WebHost
    {
        public static void Start(IMvcApplication application)
        {
            application.ConfigureServices();

            var serverRoutingTable = new ServerRoutingTable();
            AutoRegisterRoutes(serverRoutingTable, application);

            application.Configure();

            var server = new Server(80, serverRoutingTable);
            server.Run();
        }

        private static void AutoRegisterRoutes(ServerRoutingTable serverRoutingTable, IMvcApplication application)
        {
            var controllers = application.GetType().Assembly.GetTypes()
                 .Where(myType => myType.IsClass
                                  && !myType.IsAbstract
                                  && myType.IsSubclassOf(typeof(Controller)));

            foreach (var controller in controllers)
            {
                var getMethods = controller.GetMethods(BindingFlags.Public | BindingFlags.Instance).Where(
                    method => method.CustomAttributes.Any(
                        ca => ca.AttributeType.IsSubclassOf(typeof(HttpAttribute))));

                foreach (var methodInfo in getMethods)
                {
                    var httpAttribute = (HttpAttribute)methodInfo.GetCustomAttributes(true)
                        .FirstOrDefault(ca =>
                            ca.GetType().IsSubclassOf(typeof(HttpAttribute)));

                    if (httpAttribute == null)
                    {
                        continue;
                    }

                    serverRoutingTable.Add(httpAttribute.Method, httpAttribute.Path, (request) => ExecuteAction(controller, methodInfo, request));
                    //Console.WriteLine($"Route registered: {controller.Name}.{methodInfo.Name} => {httpAttribute.Method} => {httpAttribute.Path}");
                }
            }
        }

        private static IHttpResponse ExecuteAction(Type controllerType, MethodInfo methodInfo, IHttpRequest request)
        {
            var controllerInstance = Activator.CreateInstance(controllerType) as Controller;
            if (controllerInstance == null)
            {
                return new TextResult(HttpResponseStatusCode.InternalServerError, "Controller not found.");
            }

            controllerInstance.Request = request;

            var httpResponse = methodInfo.Invoke(controllerInstance, new object[] { }) as IHttpResponse;
            return httpResponse;

            //serverRoutingTable.Routes[HttpRequestMethod.GET]["/user/login"] = request => new UserController().Login(request);
            //serverRoutingTable.Routes[HttpRequestMethod.POST]["/user/login"] = request => new UserController().LoginPost(request);
            //serverRoutingTable.Routes[HttpRequestMethod.GET]["/user/register"] = request => new UserController().Register(request);
            //serverRoutingTable.Routes[HttpRequestMethod.POST]["/user/register"] = request => new UserController().RegisterPost(request);
            //serverRoutingTable.Routes[HttpRequestMethod.GET]["/user/logout"] = request => new UserController().Logout(request);
            //serverRoutingTable.Routes[HttpRequestMethod.GET]["/album/all"] = request => new AlbumController().All(request);
            //serverRoutingTable.Routes[HttpRequestMethod.GET]["/album/create"] = request => new AlbumController().Create(request);
            //serverRoutingTable.Routes[HttpRequestMethod.POST]["/album/create"] = request => new AlbumController().CreatePost(request);
            //serverRoutingTable.Routes[HttpRequestMethod.GET]["/album/details"] = request => new AlbumController().Details(request);
            //serverRoutingTable.Routes[HttpRequestMethod.GET]["/track/create"] = request => new TrackController().Create(request);
            //serverRoutingTable.Routes[HttpRequestMethod.POST]["/track/create"] = request => new TrackController().CreatePost(request);
            //serverRoutingTable.Routes[HttpRequestMethod.GET]["/track/details"] = request => new TrackController().Details(request);
        }
    }
}