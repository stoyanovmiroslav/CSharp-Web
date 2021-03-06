﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.Framework.ActionResult.Contracts
{
    public interface IViewable : IActionResult
    {
        IRenderable View { get; set; }
    }
}