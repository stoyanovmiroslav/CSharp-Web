using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.Framework.ActionResult.Contracts
{
    public interface IRedirectable : IActionResult
    {
        string RedirectUrl { get; set; }
    }
}