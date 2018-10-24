using MishMash.ViewModels.Chanel;
using System;
using System.Collections.Generic;
using System.Text;

namespace MishMash.ViewModels.Home
{
    public class HomeChanelsViewModel
    {
        public List<DetailsChanelViewModel> AllChannels { get; set; }

        public List<DetailsChanelViewModel> YourChannels { get; set; }

        public List<DetailsChanelViewModel> SuggestedChannels { get; set; }

        public List<DetailsChanelViewModel> OtherChannels { get; set; }
    }
}
