using System;

namespace SIS.Framework.Attributes
{
    public abstract class HttpMethodAttribute : Attribute
    {
        public abstract bool IsValid(string requestMethod);
    }
}