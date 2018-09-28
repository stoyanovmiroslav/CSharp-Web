using SIS.HTTP.Enums;
using SIS.HTTP.Headers;
using SIS.HTTP.Headers.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.HTTP.Responses.Contracts
{
    public interface IHttpResponse
    {
        HttpResponseStatusCode StatusCode { get; set; }

        IHttpHeaderCollection Headers { get; }

        byte[] Content { get; set; }

        void AddHeader(HttpHeader httpHeader);

        byte[] GetBytes();
    }
}