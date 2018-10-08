using System;
using System.Linq;
using System.Text;
using System.Web;
using IRunes.Models;
using Microsoft.EntityFrameworkCore;
using SIS.HTTP.Enums;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contracts;
using SIS.WebServer.Results;

namespace IRunes.Controlers
{
    public class TrackController : BaseController
    {
        public IHttpResponse Create(IHttpRequest request)
        {
            var username = this.GetUsername(request);

            if (username == null)
            {
                return this.View("User/Login");
            }

            string albumId = request.QueryData["albumId"].ToString();
            this.ViewBag["albumId"] = albumId;

            return this.View();
        }

        public IHttpResponse CreatePost(IHttpRequest request)
        {
            var username = this.GetUsername(request);

            if (username == null)
            {
                return this.View("User/Login");
            }

            string albumId = request.QueryData["albumId"].ToString();

            string name = request.FormData["name"].ToString();
            string link = request.FormData["link"].ToString();
            decimal price = decimal.Parse(request.FormData["price"].ToString());

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(link) || price == 0)
            {
                return this.View("/track/create");
            }

            var album = this.db.Albums.Include(x => x.Tracks).FirstOrDefault(x => x.Id == albumId);

            album.Tracks.Add(new Track { Name = name, Link = link, Price = price });
            db.SaveChanges();

            string albumCover = HttpUtility.UrlDecode(album.Cover);

            var tracksPrice = album.Tracks.Sum(x => x.Price);
            var tracksPriceAfterDiscount = tracksPrice - (tracksPrice * 13 / 100);

            var albumData = new StringBuilder();
            albumData.Append($"<br/><img src=\"{albumCover}\" width=\"250\" height=\"250\"><br/>");
            albumData.Append($"<b>Name: {album.Name}</b><br/>");
            albumData.Append($"<b>Price: ${tracksPriceAfterDiscount}</b><br/>");

            var tracks = album.Tracks.ToArray();

            var sbTracks = new StringBuilder();

            this.ViewBag["tracks"] = "";

            if (tracks.Length > 0)
            {
                foreach (var track in tracks)
                {
                    sbTracks.Append($"<a href=\"/track/details?id={track.Id}&albumId={albumId}\">{track.Name}</a></li><br/>");
                }

                this.ViewBag["tracks"] = sbTracks.ToString();
            }

            this.ViewBag["albumId"] = album.Id;
            this.ViewBag["album"] = albumData.ToString();

            return this.View("/album/details");
        }

        public IHttpResponse Details(IHttpRequest request)
        {
            var username = this.GetUsername(request);

            if (username == null)
            {
                return this.View("User/Login");
            }

            //TODO trackId from string to int
            var trackId = request.QueryData["id"].ToString();
            var albumId = request.QueryData["albumId"].ToString();

            var track = this.db.Tracks.FirstOrDefault(x => x.Id == int.Parse(trackId));

            string trackLink = HttpUtility.UrlDecode(track.Link);

            var trackInfo = new StringBuilder();
            trackInfo.Append($"<b>Track Name: {track.Name}</b><br/>");
            trackInfo.Append($"<b>Track Price: ${track.Price}</b><br/>");

            string trackVideo = $"<iframe class=\"embed-responsive-item\" src=\"{trackLink}\"></iframe><br/>";

            this.ViewBag["trackVideo"] = trackVideo;
            this.ViewBag["trackInfo"] = trackInfo.ToString();

            this.ViewBag["albumId"] = albumId;

            return this.View();
        }
    }
}