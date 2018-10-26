using Microsoft.EntityFrameworkCore;
using MishMash.Models;
using MishMash.ViewModels.Chanel;
using SIS.HTTP.Responses.Contracts;
using SIS.MvcFramework.HttpAttributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MishMash.Controllers
{
    public class ChannelController : BaseController
    {
        [HttpGet("/channel/follow")]
        public IHttpResponse Follow(int id)
        {
            var user = db.Users.Include(x => x.Channels)
                              .FirstOrDefault(x => x.Username == this.User.Name);

            var chanel = db.Channels.FirstOrDefault(x => x.ChannelId == id);

            if (user == null || chanel == null)
            {
                return this.BadRequestError("Invalid request!");
            }

            var userChanel = new UserChanel
            {
                User = user,
                Channel = chanel
            };

            db.UserChanels.Add(userChanel);
            db.SaveChanges();

            return this.Redirect("/");
        }

        [HttpGet("/channel/unfollow")]
        public IHttpResponse Unfollow(int id)
        {
            var user = db.Users.Include(x => x.Channels)
                              .FirstOrDefault(x => x.Username == this.User.Name);

            var chanel = user.Channels.FirstOrDefault(x => x.ChannelId == id);

            if (user == null || chanel == null)
            {
                return this.BadRequestError("Invalid operation");
            }

            db.UserChanels.Remove(chanel);
            db.SaveChanges();

            return this.Followed();
        }

        [HttpGet("/channel/followed")]
        public IHttpResponse Followed()
        {
            var user = db.Users.Include(x => x.Channels)
                               .FirstOrDefault(x => x.Username == this.User.Name);

            var followedChanels = db.UserChanels.Include(x => x.Channel)
                                                .Where(x => x.UserId == user.UserId)
                                                .Select(x => new BasicChanelViewModel
                                                {
                                                    Id = x.Channel.ChannelId,
                                                    Name = x.Channel.Name,
                                                    FollowersCount = x.Channel.Followers.Count(),
                                                    Type = x.Channel.Type.ToString()
                                                }).ToArray();

            return this.View(followedChanels);
        }

        [HttpGet("/channel/details")]
        public IHttpResponse Details(int id)
        {
            var channelViewModel = this.db.Channels.Where(x => x.ChannelId == id)
                .Select(x => new BasicChanelViewModel
                {
                    Type = x.Type.ToString(),
                    Name = x.Name,
                    Tags = x.Tags.Select(t => t.Tag.Name).ToArray(),
                    Description = x.Description,
                    FollowersCount = x.Followers.Count(),
                }).FirstOrDefault();

            if (channelViewModel == null)
            {
                return this.BadRequestError("Invalid Chanel!", "/Home/Index-guest");
            }

            return this.View(channelViewModel);
        }

        [HttpGet("/channel/create")]
        public IHttpResponse Create()
        {
            return this.View();
        }

        [HttpPost("/channel/create")]
        public IHttpResponse Create(CreateChanelViewModel model)
        {
            if (!Enum.TryParse(model.Type, true, out MishMash.Models.Enums.Type type))
            {
                return this.BadRequestError("Invalid Type", "/channel/create");
            }

            var tagsAsString = model.Tags.Split(",", StringSplitOptions.RemoveEmptyEntries)
                                         .Select(x => x.Trim())
                                         .Where(x => !string.IsNullOrWhiteSpace(x))
                                         .ToArray();

            var tags = new List<ChanelTag>();
            var channel = new Channel();

            foreach (var tagAsString in tagsAsString)
            {
                Tag tag = db.Tags.FirstOrDefault(x => x.Name == tagAsString);

                if (tag == null)
                {
                    tag = new Tag { Name = tagAsString };
                }

                var chanelTag = new ChanelTag
                {
                    Tag = tag,
                    Channel = channel
                };

                tags.Add(chanelTag);
            }


            channel.Name = model.Name;
            channel.Description = model.Description;
            channel.Tags = tags;
            channel.Type = type;


            db.Channels.Add(channel);
            db.SaveChanges();

            return this.Redirect("/");
        }
    }
}