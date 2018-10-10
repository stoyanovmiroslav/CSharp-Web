using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.HTTP.Headers
{
    public class HttpHeader
    {
        public const string COOKIE = "Cookie";

        public const string CONTENT_TYPE = "Content-Type";

        public const string CONTENT_LENGTH = "Content-Length";

        public const string CONTENT_DISPOSITION = "Content-Disposition";

        public const string AUTHORIZATION = "Authorization";

        public const string HOST = "Host";

        public const string LOCATION = "Location";

        public const string CONTENT_LOCATION = "Content-Location";

        public HttpHeader(string key, string value)
        {
            this.Key = key;
            this.Value = value;
        }

        public string Key { get; set; }

        public string Value { get; set; }

        public override string ToString()
        {
            return $"{this.Key}: {this.Value}";
        }
    }
}