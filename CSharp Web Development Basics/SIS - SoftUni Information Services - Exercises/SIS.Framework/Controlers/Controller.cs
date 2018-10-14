using SIS.Framework.ActionResult;
using SIS.Framework.ActionResult.Contracts;
using SIS.Framework.Utilities;
using SIS.Framework.Views;
using SIS.HTTP.Requests.Contracts;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace SIS.Framework.Controlers
{
    public class Controller
    {
        public IHttpRequest Request { get; set; }

        protected IViewable View([CallerMemberName] string caller = "")
        {
            var controllerName = ControllerUtilities.GetContorlerName(this);

            var fullQualifiedName = ControllerUtilities.GetFullQualifiedName(controllerName, caller);

            var view = new View(fullQualifiedName);

            return new ViewResult(view);
        }

        protected IRedirectable RedirectToAction(string redirectUrl) 
            => new RedirectResult(redirectUrl);

    }
}