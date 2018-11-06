using Panda.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Panda.ViewModels.Home
{
    public class HomeViewModel
    {
        public Package[] Pending { get; set; }

        public Package[] Shipped { get; set; }

        public Package[] Delivered { get; set; }
    }
}
