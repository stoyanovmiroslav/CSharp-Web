using System.Collections.Generic;

namespace IRunes.ViewModels.Album
{
    public class AlbumDetailsViewModel
    {
        public string AlbumId { get; set; }

        public string AlbumName { get; set; }

        public string AlbumCover { get; set; }

        public decimal TracksPriceAfterDiscount { get; set; }

        public List<IRunes.Models.Track> Tracks  { get; set; }
    }
}