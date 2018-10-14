using SIS.HTTP.Exceptions;
using SIS.HTTP.Headers.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.HTTP.Headers
{
    public class HttpHeaderCollection : IHttpHeaderCollection
    {
        private readonly Dictionary<string, HttpHeader> headers;

        public HttpHeaderCollection()
        {
            this.headers = new Dictionary<string, HttpHeader>();
        }

        public void Add(HttpHeader httpHeader)
        {
            this.headers[httpHeader.Key] = httpHeader;
        }

        public bool ContainsHeader(string key)
        {
            return this.headers.ContainsKey(key);
        }

        public HttpHeader GetHeader(string key)
        {
            if (!ContainsHeader(key))
            {
                throw new BadRequestException();
            }

            return this.headers[key];
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (var header in this.headers)
            {
                stringBuilder.AppendLine($"{header.Value}");
            }

            return stringBuilder.ToString().TrimEnd();
        }
    }
}