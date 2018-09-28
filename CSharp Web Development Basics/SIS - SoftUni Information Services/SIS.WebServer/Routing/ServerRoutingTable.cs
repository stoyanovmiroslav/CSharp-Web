using SIS.HTTP.Enums;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.WebServer.Routing
{
    public class ServerRoutingTable
    {
        public ServerRoutingTable()
        {
            this.Routes = new Dictionary<HttpRequestMethod, Dictionary<string, Func<IHttpRequest, IHttpResponse>>>()
            {
                [HttpRequestMethod.GET] = new Dictionary<string, Func<IHttpRequest, IHttpResponse>>(),
                [HttpRequestMethod.POST] = new Dictionary<string, Func<IHttpRequest, IHttpResponse>>(),
                [HttpRequestMethod.PUT] = new Dictionary<string, Func<IHttpRequest, IHttpResponse>>(),
                [HttpRequestMethod.DELETE] = new Dictionary<string, Func<IHttpRequest, IHttpResponse>>()
            };
        }

        public Dictionary<HttpRequestMethod, Dictionary<string,Func<IHttpRequest, IHttpResponse>>> Routes { get; }
    }
}