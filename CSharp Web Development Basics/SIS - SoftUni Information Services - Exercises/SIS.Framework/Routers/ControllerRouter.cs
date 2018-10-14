using SIS.Framework.ActionResult.Contracts;
using SIS.Framework.Attributes;
using SIS.Framework.Controlers;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses;
using SIS.HTTP.Responses.Contracts;
using SIS.WebServer.Api.Contracts;
using SIS.WebServer.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SIS.Framework.Routers
{
    public class ControllerRouter : IHttpHandler
    {
        public IHttpResponse HandlerRequest(IHttpRequest request)
        {
            var controllerName = string.Empty;
            var actionName = string.Empty;
            var requestMethod = request.RequestMethod.ToString();

            if (request.Url == "/")
            {
                controllerName = "HomeController";
                actionName = "Index";
            }
            else
            {
                var requestUrlSplit = request.Url.Split("/", StringSplitOptions.RemoveEmptyEntries);

                if (requestUrlSplit.Length >= 2)
                {
                    controllerName = requestUrlSplit[0];
                    actionName = requestUrlSplit[1];
                }
            }

            var controller = this.GetController(controllerName, request);

            var action = this.GetAction(requestMethod, controller, actionName);

            if (controller == null || action == null)
            {
                return new HttpResponse(HTTP.Enums.HttpResponseStatusCode.NotFound);
            }

            return this.PrepareResponse(controller, action);
        }

        private IHttpResponse PrepareResponse(Controller controller, MethodInfo action)
        {
            IActionResult actionResult = (IActionResult)action.Invoke(controller, null);
            string invocationResult = actionResult.Invoke();

            if (actionResult is IViewable)
            {
                return new HtmlResult(HTTP.Enums.HttpResponseStatusCode.Ok, invocationResult);
            }
            else if (actionResult is IRenderable)
            {
                return new RedirectResult(invocationResult);
            }
            else
            {
                throw new InvalidOperationException("The view result is not supported.");
            }
        }

        private MethodInfo GetAction(string requestMethod, Controller controller, string actionName)
        {
            var actions = this.GetSuitableMethods(controller, actionName) .ToList();

            if (!actions.Any())
            {
                return null;
            }

            foreach (var action in actions)
            {
                var httpMethodAttributes = action.GetCustomAttributes()
                                                  .Where(ca => ca is HttpMethodAttribute)
                                                  .Cast<HttpMethodAttribute>()
                                                  .ToList();

                if (!httpMethodAttributes.Any() && requestMethod.ToLower() == "get")
                {
                    return action;
                }

                foreach (var httpMethodAttribute in httpMethodAttributes)
                {
                    if (httpMethodAttribute.IsValid(requestMethod))
                    {
                        return action;
                    }
                }
            }

            return null;
        }

        private IEnumerable<MethodInfo> GetSuitableMethods(Controller controller, string actionName)
        {
            if (controller == null)
            {
                return new MethodInfo[0];
            }

            return controller.GetType().GetMethods()
                                       .Where(mi => mi.Name.ToLower() == actionName.ToLower());
        }

        private Controller GetController(string controllerName, IHttpRequest request)
        {
            if (string.IsNullOrWhiteSpace(controllerName))
            {
                return null;
            }

            var fullyQualifiedControllerName = string.Format("{0}.{1}.{2}, {0}",
                MvcContext.Get.AssemblyName,
                MvcContext.Get.ControllersFolder,
                controllerName);

            var controllerType = Type.GetType(fullyQualifiedControllerName);

            var controller = (Controller)Activator.CreateInstance(controllerType);
            return controller;
        }
    }
}