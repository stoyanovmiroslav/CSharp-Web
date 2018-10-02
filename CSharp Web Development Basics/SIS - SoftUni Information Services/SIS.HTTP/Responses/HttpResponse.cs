﻿using SIS.HTTP.Common;
using SIS.HTTP.Enums;
using SIS.HTTP.Headers;
using SIS.HTTP.Headers.Contracts;
using SIS.HTTP.Responses.Contracts;
using SIS.HTTP.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIS.HTTP.Cookies;
using SIS.HTTP.Cookies.Contracts;

namespace SIS.HTTP.Responses
{
    public class HttpResponse : IHttpResponse
    {
        public HttpResponse()
        {
        }

        public HttpResponse(HttpResponseStatusCode statusCode)
        {
            this.Headers = new HttpHeaderCollection();
            this.Cookies = new HttpCookieCollection();
            this.Content = new byte[0];
            this.StatusCode = statusCode;
        }

        public HttpResponseStatusCode StatusCode { get; set; }

        public IHttpHeaderCollection Headers { get; private set; }

        public IHttpCookieCollection Cookies { get; private set; }

        public byte[] Content { get; set; }

        public void AddCookie(HttpCookie httpCookie)
        {
            this.Cookies.Add(httpCookie);
        }

        public void AddHeader(HttpHeader httpHeader)
        {
            this.Headers.Add(httpHeader);
        }

        public byte[] GetBytes()
        {
            return Encoding.UTF8.GetBytes(this.ToString()).Concat(this.Content).ToArray();
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"{GlobalConstans.HttpOneProtocolFragment} {this.StatusCode.GetResponseLine()}")
                         .AppendLine($"{this.Headers}");

            if (this.Cookies.HasCookies())
            {
                stringBuilder.AppendLine($"Set-Cookie: {this.Cookies}");
            }

            stringBuilder.AppendLine();

            return stringBuilder.ToString();
        }
    }
}