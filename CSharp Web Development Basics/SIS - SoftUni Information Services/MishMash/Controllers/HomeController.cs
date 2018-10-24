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

            var yourChannels = db.Channels.Where(x => x.Followers.Any(f => f.User.Username == this.User))
                                         .Select(x => new DetailsChanelViewModel
                                                     {
                                                         Id = x.ChannelId,
                                                         Name = x.Name,
                                                         Type = x.Type.ToString(),
                                                         FollowersCount = x.Followers.Count()
                                                     }).ToList();

            var suggestedChannels = db.Channels.Where(x => x.Followers.Any(f => f.User.Username == this.User))
                                         .Select(x => new DetailsChanelViewModel
                                         {
                                             Id = x.ChannelId,
                                             Name = x.Name,
                                             Type = x.Type.ToString(),
                                             FollowersCount = x.Followers.Count()
                                         }).ToList();

            var otherChannels = db.Channels.Where(x => x.Followers.Any(f => f.User.Username == this.User))
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