using SIS.HTTP.Enums;
using SIS.HTTP.Extensions;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contracts;
using SIS.MvcFramework.Contracts;
using SIS.MvcFramework.HttpAttributes;
using SIS.MvcFramework.Services;
using SIS.MvcFramework.Services.Contracts;
using SIS.WebServer;
using SIS.WebServer.Results;
using SIS.WebServer.Routing;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace SIS.MvcFramework
{
    public static class WebHost
    {
        public static void Start(IMvcApplication application)
        {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;

            var serviceCollection  = new ServiceCollection();
            application.ConfigureServices(serviceCollection );

            var serverRoutingTable = new ServerRoutingTable();
            AutoRegisterRoutes(serverRoutingTable, application, serviceCollection);

            application.Configure();

            var server = new Server(80, serverRoutingTable);
            server.Run();
        }

        private static void AutoRegisterRoutes(ServerRoutingTable serverRoutingTable, IMvcApplication application, IServiceCollection serviceCollection)
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

                    var path = httpAttribute.Path;
                    if (!path.StartsWith("/"))
                    {
                        path = "/" + path;
                    }

                    serverRoutingTable.Add(httpAttribute.Method, path, (request) => ExecuteAction(controller, methodInfo, request, serviceCollection));
                }
            }
        }

        private static IHttpResponse ExecuteAction(Type controllerType, MethodInfo methodInfo, IHttpRequest request, IServiceCollection serviceCollection)
        {
            var controllerInstance = serviceCollection.CreateInstance(controllerType) as Controller;

            if (controllerInstance == null)
            {
                return new TextResult(HttpResponseStatusCode.InternalServerError, "Controller not found.");
            }

            controllerInstance.Request = request;
            controllerInstance.UserCookieService = serviceCollection.CreateInstance<IUserCookieService>();

            var actionParameters = methodInfo.GetParameters();
            var actionParameterObjects = new List<object>();

            foreach (var actionParameter in actionParameters)
            {
                if (actionParameter.ParameterType.IsValueType ||
                    Type.GetTypeCode(actionParameter.ParameterType) == TypeCode.String)
                {
                    var stringValue = GetRequestData(request, actionParameter.Name);
                    actionParameterObjects.Add(TryChangeType(stringValue, actionParameter.ParameterType));
                }
                else
                {
                    var instance = serviceCollection.CreateInstance(actionParameter.ParameterType);

                    var properties = instance.GetType().GetProperties();

                    foreach (var property in properties)
                    {
                        string stringValue = GetRequestData(request, property.Name);

                        if (stringValue != null)
                        {
                            var value = TryChangeType(stringValue, property.PropertyType);
                            property.SetValue(instance, value);
                        }
                    }

                    actionParameterObjects.Add(instance);
                }
            }

            var httpResponse = methodInfo.Invoke(controllerInstance, actionParameterObjects.ToArray()) as IHttpResponse;
            return httpResponse;
        }

        private static object TryChangeType(string stringValue, Type propertyType)
        {
            try
            {
                return Convert.ChangeType(stringValue, propertyType);
            }
            catch
            {
                if (propertyType.IsValueType)
                {
                    return Activator.CreateInstance(propertyType);
                }
            }
            
            return null;
        }

        private static string GetRequestData(IHttpRequest request, string key)
        {
            key = key.ToLower();
            string stringValue = null;

            if (request.FormData.Any(x => x.Key.ToLower() == key))
            {
                stringValue = request.FormData
                                     .First(x => x.Key.ToLower() == key)
                                     .Value.ToString().UrlDecode();
            }
            else if (request.QueryData.Any(x => x.Key.ToLower() == key))
            {
                stringValue = request.QueryData
                                     .First(x => x.Key.ToLower() == key)
                                     .Value.ToString().UrlDecode();
            }

            return stringValue;
        }
    }
}