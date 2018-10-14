using SIS.HTTP.Cookies.Contracts;
using SIS.HTTP.Enums;
using SIS.HTTP.Headers.Contracts;
using SIS.HTTP.Session.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.HTTP.Requests.Contracts
{
    public interface IHttpRequest
    {
        string Path { get; }

        string Url { get; }

        IHttpSession Session { get; set; }

        Dictionary<string, object> FormData { get; }

        Dictionary<string, object> QueryData { get; }

        IHttpHeaderCollection Headers { get; }

        HttpRequestMethod RequestMethod { get; }

        IHttpCookieCollection Cookies { get; }
    }
}
