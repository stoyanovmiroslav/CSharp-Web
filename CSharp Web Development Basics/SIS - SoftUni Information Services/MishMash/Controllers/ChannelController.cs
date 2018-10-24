using Microsoft.EntityFrameworkCore;
using MishMash.Models;
using MishMash.Models.Enums;
using MishMash.ViewModels.Chanel;
using SIS.HTTP.Responses.Contracts;
using SIS.MvcFramework.HttpAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MishMash.Controllers
{
    public class ChannelController : BaseController
    {
        [HttpGet("/channel/follow")]
        public IHttpResponse Follow(int id)
        {
            var user = db.Users.Include(x => x.Channels)
                              .FirstOrDefault(x => x.Username == this.User);

            var chanel = db.Channels.FirstOrDefault(x => x.ChannelId == id);

            if (user == null || chanel == null)
            {
                return this.BadRequestError("Invalid operation", "/home/index");
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
                              .FirstOrDefault(x => x.Username == this.User);

            var chanel = user.Channels.FirstOrDefault(x => x.ChannelId == id);

            if (user == null || chanel == null)
            {
                return this.BadRequestError("Invalid operation", "/home/index");
            }

            db.UserChanels.Remove(chanel);
            db.SaveChanges();

            return this.Followed();
        }

        [HttpGet("/channel/followed")]
        public IHttpResponse Followed()
        {
            var user = db.Users.Include(x => x.Channels)
                               .FirstOrDefault(x => x.Username == this.User);

            var followedChanels = db.UserChanels.Include(x => x.Channel)
                                                .Where(x => x.UserId == user.UserId)
                                                .Select(x => new DetailsChanelViewModel
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
            var chanel = db.Channels.Include(x => x.Tags)
                                    .Include(x => x.Followers)
                                    .FirstOrDefault(x => x.ChannelId == id);

            if (chanel == null)
            {
                return this.BadRequestError("Invalid Chanel!", "/Home/Index-guest");
            }
            
            return this.View(chanel);
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
            var tags = new List<Tag>();

            foreach (var tagAsString in tagsAsString)
            {
                Tag tag = db.Tags.FirstOrDefault(x => x.Name == tagAsString);

                if (tag == null)
                {
                    tag = new Tag { Name = tagAsString };
                }

                tags.Add(tag);
            }

            var channel = new Channel
            {
                Name = model.Name,
                Description = model.Description,
                Tags = tags,
                Type = type,
            };

            db.Channels.Add(channel);
            db.SaveChanges();

            return this.View();
        }
    }
}