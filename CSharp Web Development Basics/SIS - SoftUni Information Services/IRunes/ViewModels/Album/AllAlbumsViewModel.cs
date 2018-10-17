using System.Collections.Generic;

namespace IRunes.ViewModels.Album
{
    public class AllAlbumsViewModel
    {
        public string MessageForNoAnyAlbums => "There are currently no albums.";

        public List<IRunes.Models.Album> Albums { get; set; }
    }
}