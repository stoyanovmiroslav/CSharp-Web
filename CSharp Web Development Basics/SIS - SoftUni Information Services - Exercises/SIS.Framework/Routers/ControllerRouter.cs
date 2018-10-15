using SIS.Framework.ActionResult.Contracts;
using SIS.Framework.Attributes;
using SIS.Framework.Controlers;
using SIS.HTTP.Extensions;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses;
using SIS.HTTP.Responses.Contracts;
using SIS.WebServer.Api.Contracts;
using SIS.WebServer.Results;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

            if (request.Path == "/")
            {
                controllerName = "Home";
                actionName = "Index";
            }
            else
            {
                var requestUrlSplit = request.Path.Split("/", StringSplitOptions.RemoveEmptyEntries);

                if (requestUrlSplit.Length >= 2)
                {
                    controllerName = requestUrlSplit[0].Capitalize();
                    actionName = requestUrlSplit[1];
                }
            }

            var controller = this.GetController(controllerName, request);
            var action = this.GetAction(requestMethod, controller, actionName);

            if (controller == null || action == null)
            {
                return new HttpResponse(HTTP.Enums.HttpResponseStatusCode.NotFound);
            }

            controller.Request = request;

            object[] actionParameters = this.MapActionParameters(action, request, controller);

            IActionResult actionResult = (IActionResult)action.Invoke(controller, actionParameters);

            var response = this.PrepareResponse(actionResult);

            foreach (var cookie in controller.Cookies)
            {
                response.AddCookie(cookie);
            }

            return response;
        }

        private object[] MapActionParameters(MethodInfo action, IHttpRequest request, Controller controller)
        {
            var actionParameters = action.GetParameters();

            var mappedActionParameters = new List<object>();

            foreach (var actionParameter in actionParameters)
            {
                if (actionParameter.ParameterType.IsValueType ||
                    actionParameter.ParameterType == typeof(string))
                {
                    mappedActionParameters.Add(ProcessPrimitiveParameters(actionParameter, request));
                }
                else
                {
                    var bindingModel = ProcessBindingModelParameters(actionParameter, request);

                    controller.ModelState.IsValid = IsValidModel(bindingModel);
                    mappedActionParameters.Add(bindingModel);
                }
            }

            return mappedActionParameters.ToArray();
        }

        private object ProcessBindingModelParameters(ParameterInfo actionParameter, IHttpRequest request)
        {
            Type bindingModelType = actionParameter.ParameterType;

            var bindingModelInstance = Activator.CreateInstance(bindingModelType);

            var bindingModelProperties = bindingModelType.GetProperties();

            foreach (var property in bindingModelProperties)
            {
                try
                {
                    object value = this.GetParametersFromRequestData(property.Name, request);
                    property.SetValue(bindingModelInstance, Convert.ChangeType(value, property.PropertyType));
                }
                catch
                {
                    Console.WriteLine($"The {property.Name} field could not be mapped.");
                }
            }

            return Convert.ChangeType(bindingModelInstance, bindingModelType);
        }

        private object ProcessPrimitiveParameters(ParameterInfo actionParameter, IHttpRequest request)
        {
            object value = GetParametersFromRequestData(actionParameter.Name, request);

            return Convert.ChangeType(value, actionParameter.ParameterType);
        }

        private object GetParametersFromRequestData(string parameterName, IHttpRequest request)
        {
            string key = parameterName.ToLower();

            if (request.QueryData.Any(x => x.Key.ToLower() == key))
            {
                return request.QueryData.First(x => x.Key.ToLower() == key).Value;
            }
            else if (request.FormData.Any(x => x.Key.ToLower() == key))
            {
                return request.FormData.First(x => x.Key.ToLower() == key).Value;
            }

            return null;
        }

        private IHttpResponse PrepareResponse(IActionResult actionResult)
        {
            string invocationResult = actionResult.Invoke();

            if (actionResult is IViewable)
            {
                return new HtmlResult(HTTP.Enums.HttpResponseStatusCode.Ok, invocationResult);
            }
            else if (actionResult is IRedirectable)
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
            var actions = this.GetSuitableMethods(controller, actionName).ToList();

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

            var fullyQualifiedControllerName = string.Format("{0}.{1}.{2}{3}, {0}",
                MvcContext.Get.AssemblyName,
                MvcContext.Get.ControllersFolder,
                controllerName,
                MvcContext.Get.ControllerSuffix);

            var controllerType = Type.GetType(fullyQualifiedControllerName);

            var controller = (Controller)Activator.CreateInstance(controllerType);
            return controller;
        }

        private static bool IsValidModel(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResults = new List<ValidationResult>();

            return System.ComponentModel.DataAnnotations.Validator.TryValidateObject(obj, validationContext, validationResults, true);
        }
    }
}