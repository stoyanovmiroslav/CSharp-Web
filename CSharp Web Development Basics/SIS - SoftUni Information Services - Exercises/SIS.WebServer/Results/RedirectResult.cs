using SIS.HTTP.Enums;
using SIS.HTTP.Responses;
using SIS.HTTP.Headers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.WebServer.Results
{
    public class RedirectResult : HttpResponse
    {
        public RedirectResult(string location)
            : base(HttpResponseStatusCode.SeeOther)
        {
            this.Headers.Add(new HttpHeader("Location", location));
        }
    }
}