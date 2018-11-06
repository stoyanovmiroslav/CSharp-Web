using System;
using System.Collections.Generic;
using System.Text;

namespace MeTube.ViewModels.Tube
{
    public class TubeViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public string Description { get; set; }

        public string YoutubeId { get; set; }

        public string YoutubeLink { get; set; }
    }
}