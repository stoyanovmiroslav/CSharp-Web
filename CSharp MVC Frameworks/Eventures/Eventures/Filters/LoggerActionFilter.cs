using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.VisualStudio.Web.CodeGeneration;
using System;
using System.Linq;

namespace Eventures.Filters
{
    public class LoggerActionFilter : ActionFilterAttribute
    {
        private ILogger logger;

        public LoggerActionFilter(ILogger logger)
        {
            this.logger = logger;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.ModelState.IsValid)
            {
                var modelData = context.ModelState.Values.Select(x => x.AttemptedValue).ToArray();

                if (modelData.Count() > 1)
                {
                    var eventName = modelData[1];
                    var eventStart = modelData[3];
                    var eventEnd= modelData[0];

                    var administrator = context.HttpContext.User.Identity.Name;
                    var massage = $"[{DateTime.UtcNow}] Administrator {administrator} create event {eventName} ({eventStart} / {eventEnd}).";

                    this.logger.LogMessage(massage, LogMessageLevel.Information);
                }
            }
        }
    }
}