using SIS.HTTP.Enums;
using SIS.HTTP.Headers;
using SIS.HTTP.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.WebServer.Results
{
    public class UnauthorizedResult : HttpResponse
    {
        private const string DefaultErrorHeading = "<h1>You have no premission to access this functionality</h1>";

        public UnauthorizedResult()
            : base(HttpResponseStatusCode.Unauthorized)
        {
            this.Headers.Add(new HttpHeader("Content-Type", "text/html"));
            this.Content = Encoding.UTF8.GetBytes(DefaultErrorHeading);
        }
    }
}
