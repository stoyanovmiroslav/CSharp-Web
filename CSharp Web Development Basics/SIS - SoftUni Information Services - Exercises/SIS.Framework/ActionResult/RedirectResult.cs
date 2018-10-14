using SIS.Framework.ActionResult.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.Framework.ActionResult
{
    public class RedirectResult : IRedirectable
    {
        public RedirectResult(string redirectUrl)
        {
            this.RedirectUrl = redirectUrl;
        }

        public string RedirectUrl { get; set; }

        public string Invoke()
        {
            return this.RedirectUrl;
        }
    }
}