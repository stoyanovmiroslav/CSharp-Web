﻿using System.Linq;
using System.Web;
using IRunes.Models;
using IRunes.ViewModels.Album;
using IRunes.ViewModels.Track;
using Microsoft.EntityFrameworkCore;
using SIS.HTTP.Responses.Contracts;
using SIS.MvcFramework.HttpAttributes;

namespace IRunes.Controlers
{
    public class TrackController : BaseController
    {
        [HttpGet("/track/create")]
        public IHttpResponse Create(CreateTrackViewModel model)
        {
            if (this.User == null)
            {
                return this.Redirect("/user/login");
            }

            return this.View(model);
        }

        [HttpPost("/track/create")]
        public IHttpResponse Create(CreateTrackActionViewModel model)
        {
            if (this.User == null)
            {
                return this.Redirect("/user/login");
            }

            if (string.IsNullOrWhiteSpace(model.Name) || string.IsNullOrWhiteSpace(model.Link) || model.Price == 0)
            {
                return this.Redirect("/track/create");
            }

            var album = this.db.Albums.Include(x => x.Tracks).FirstOrDefault(x => x.Id == model.AlbumId);

            album.Tracks.Add(new Track { Name = model.Name, Link = model.Link, Price = model.Price });
            db.SaveChanges();

            var tracksPrice = album.Tracks.Sum(x => x.Price);
            var tracksPriceAfterDiscount = tracksPrice - (tracksPrice * 13 / 100);

            AlbumDetailsViewModel albumModel = new AlbumDetailsViewModel
            {
                AlbumCover = HttpUtility.UrlDecode(album.Cover),
                AlbumName = album.Name,
                AlbumId = album.Id,
                TracksPriceAfterDiscount = tracksPriceAfterDiscount,
                Tracks = album.Tracks.ToList()
            };

            return this.View("/album/details", albumModel);
        }

        [HttpGet("/track/details")]
        public IHttpResponse Details(DetailsTrackViewModel model)
        {
            if (this.User == null)
            {
                return this.Redirect("/user/login");
            }

            var track = this.db.Tracks.FirstOrDefault(x => x.Id == model.TrackId);

            string trackLink = HttpUtility.UrlDecode(track.Link);

            //this.ViewBag["albumId"] = model.AlbumId;

            //this.ViewBag["trackLink"] = trackLink;
            //this.ViewBag["trackVideo"] = this.ReadFile("/Parts/Video");

            //this.ViewBag["trackName"] = track.Name;
            //this.ViewBag["trackPrice"] = track.Price.ToString();
            //this.ViewBag["trackInfo"] = this.ReadFile("/Parts/TrackInfo");

            return this.View();
        }
    }
}