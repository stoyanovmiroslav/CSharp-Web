using SIS.HTTP.Common;
using SIS.HTTP.Enums;
using SIS.HTTP.Headers;
using SIS.HTTP.Headers.Contracts;
using SIS.HTTP.Responses.Contracts;
using SIS.HTTP.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            this.Content = new byte[0];
            this.StatusCode = statusCode;
        }

        public HttpResponseStatusCode StatusCode { get; set; }

        public IHttpHeaderCollection Headers { get; private set; }

        public byte[] Content { get; set; }

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
                         .AppendLine($"{this.Headers}")
                         .AppendLine();

            return stringBuilder.ToString();
        }
    }
}