﻿using Microsoft.EntityFrameworkCore;
using MishMash.ViewModels.Chanel;
using MishMash.ViewModels.Home;
using SIS.HTTP.Responses;
using SIS.HTTP.Responses.Contracts;
using SIS.MvcFramework.HttpAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MishMash.Controllers
{
    public class HomeController : BaseController
    {
        [HttpGet("/")]
        public IHttpResponse Index()
        {
            if (this.User == null)
            {
                return this.View("/Home/Index-guest");
            }

            var allChannels = db.Channels.Select(x => new DetailsChanelViewModel
                                                     {
                                                         Id = x.ChannelId,
                                                         Name = x.Name,
                                                         Type = x.Type.ToString(),
                                                         FollowersCount = x.Followers.Count()
                                                     } ).ToList();

            var yourChannels = db.Channels.Include(x => x.Tags)
                                          .Where(x => x.Followers.Any(f => f.User.Username == this.User))
                                          .Select(x => new DetailsChanelViewModel
                                                     {
                                                         Id = x.ChannelId,
                                                         Name = x.Name,
                                                         Type = x.Type.ToString(),
                                                         FollowersCount = x.Followers.Count(),
                                                         TagsId = x.Tags.Select(t => t.TagId).ToList()
                                                     }).ToList();

            var yourChannelTags = yourChannels.SelectMany(x => x.TagsId).ToList();

            var suggestedChannels = db.Channels.Where(x => x.Tags.Any(t => yourChannelTags.Contains(t.TagId))
                                                  && !yourChannels.Any(c => c.Id == x.ChannelId))
                                         .Select(x => new DetailsChanelViewModel
                                         {
                                             Id = x.ChannelId,
                                             Name = x.Name,
                                             Type = x.Type.ToString(),
                                             FollowersCount = x.Followers.Count()
                                         }).ToList();

            var otherChannels = db.Channels.Where(x => !suggestedChannels.Any(a => a.Id == x.ChannelId)
                                            && !yourChannels.Any(a => a.Id == x.ChannelId))
                                       .Select(x => new DetailsChanelViewModel
                                       {
                                           Id = x.ChannelId,
                                           Name = x.Name,
                                           Type = x.Type.ToString(),
                                           FollowersCount = x.Followers.Count()
                                       }).ToList();

            var model = new HomeChanelsViewModel
            {
                 AllChannels = allChannels,
                 YourChannels = yourChannels,
                 SuggestedChannels = suggestedChannels,
                 OtherChannels = otherChannels
            };

            return this.View(model);
        }
    }
}