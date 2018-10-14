using SIS.HTTP.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.Framework.Attributes
{
    public class HttpPostAttribute : HttpMethodAttribute
    {
        public override bool IsValid(string requestMethod)
        {
            if (requestMethod.ToUpper() == HttpRequestMethod.POST.ToString())
            {
                return true;
            }

            return false;
        }
    }
}
