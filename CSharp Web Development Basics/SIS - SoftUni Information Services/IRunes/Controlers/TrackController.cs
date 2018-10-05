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
        public IHttpResponse All(IHttpRequest request)
        {
            this.ViewBag["albums"] = "There are currently no albums.";

            string albumsParameters = null;

            var username = this.GetUsername(request);

            if (username == null)
            {
                return this.View("User/Login");
            }

            var user = db.Users.Include(x => x.Albums).FirstOrDefault(x => x.Username == username);

            if (user == null)
            {
                return this.View("User/Login");
            }

            var albums = user.Albums.ToArray();

            foreach (var album in albums)
            {
                albumsParameters += $"<a href=\"/album/details?id={album.Id}\">{album.Name}</a></li><br/>";
            }

            if (albumsParameters != null)
            {
                this.ViewBag["albums"] = albumsParameters;
            }

            return this.View("/album/all");
        }

        public IHttpResponse Create(IHttpRequest request)
        {
            string albumId = request.QueryData["albumId"].ToString();
            this.ViewBag["albumId"] = albumId;

            return this.View("/Track/Create");
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

            var album = db.Albums.FirstOrDefault(x => x.Id == albumId);

            album.Tracks.Add(new Track{ Name = name, Link = link, Price = price });
            db.SaveChanges();

            var response = All(request);
            return response;
        }

        public IHttpResponse Details(IHttpRequest request)
        {
            //TODO Id to String
            var trackId = request.QueryData["id"].ToString();
            var albumId = request.QueryData["albumId"].ToString();

            var track = this.db.Tracks.FirstOrDefault(x => x.Id == int.Parse(trackId));

            string trackLink = HttpUtility.UrlDecode(track.Link);

            var trackData = new StringBuilder();
            trackData.Append($"<iframe width=\"300\" height=\"220\" src=\"{trackLink}\"></iframe><br/>");
            trackData.Append($"<b>Name: {track.Name}</b><br/>");
            trackData.Append($"<b>Price: ${track.Price}</b><br/>");

            this.ViewBag["trackData"] = trackData.ToString();
            this.ViewBag["albumId"] = albumId;

            return this.View("Track/Details");
        }
    }
}