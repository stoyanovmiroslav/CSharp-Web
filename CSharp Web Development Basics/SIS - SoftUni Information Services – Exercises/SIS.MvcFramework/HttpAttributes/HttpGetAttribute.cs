using SIS.HTTP.Enums;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace SIS.MvcFramework.HttpAttributes
{
    public class HttpGetAttribute : HttpAttribute
    {
        public HttpGetAttribute(string path)
            : base(path)
        {
        }

        public override HttpRequestMethod Method => HttpRequestMethod.GET;
    }
}
