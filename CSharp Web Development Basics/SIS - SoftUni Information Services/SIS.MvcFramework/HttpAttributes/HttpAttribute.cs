using SIS.HTTP.Enums;
using System;

namespace SIS.MvcFramework.HttpAttributes
{
    public abstract class HttpAttribute : Attribute
    {
        public HttpAttribute(string path)
        {
            this.Path = path;
        }

        public string Path { get; set; }

        public abstract HttpRequestMethod Method { get; }
    }
}