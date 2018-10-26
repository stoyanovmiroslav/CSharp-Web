using MishMash.ViewModels.Chanel;
using System;
using System.Collections.Generic;
using System.Text;

namespace MishMash.ViewModels.Home
{
    public class HomeChanelsViewModel
    {
        public List<BasicChanelViewModel> YourChannels { get; set; }

        public List<BasicChanelViewModel> SuggestedChannels { get; set; }

        public List<BasicChanelViewModel> OtherChannels { get; set; }
    }
}