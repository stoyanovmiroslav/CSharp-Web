using SIS.HTTP.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.Framework.Attributes
{
    public class HttpPutAttribute : HttpMethodAttribute
    {
        public override bool IsValid(string requestMethod)
        {
            if (requestMethod.ToUpper() == HttpRequestMethod.PUT.ToString())
            {
                return true;
            }

            return false;
        }
    }
}
