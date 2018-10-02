using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.HTTP.Headers
{
    public class HttpHeader
    {
        public const string Cookie = "Cookie";

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