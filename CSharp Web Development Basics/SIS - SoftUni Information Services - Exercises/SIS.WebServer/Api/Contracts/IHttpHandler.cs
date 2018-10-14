using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.WebServer.Api.Contracts
{
    public interface IHttpHandler
    {
        IHttpResponse HandlerRequest(IHttpRequest httpRequest);
    }
}