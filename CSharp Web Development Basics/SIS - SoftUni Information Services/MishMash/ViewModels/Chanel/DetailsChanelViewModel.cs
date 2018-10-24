using System;
using System.Collections.Generic;
using System.Text;

namespace MishMash.ViewModels.Chanel
{
    public class DetailsChanelViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Type { get; set; }

        public string[] Tags { get; set; }

        public int FollowersCount { get; set; }

        public List<int> TagsId { get; set; }
    }
}
