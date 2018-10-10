using SIS.HTTP.Enums;
using System;
using System.Collections.Generic;
using System.Text;

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