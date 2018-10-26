using System.Collections.Generic;

namespace MishMash.ViewModels.Chanel
{
    public class BasicChanelViewModel
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