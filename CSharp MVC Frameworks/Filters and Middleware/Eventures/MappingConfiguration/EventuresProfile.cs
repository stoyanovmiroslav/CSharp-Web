using AutoMapper;
using Eventures.Models;
using Eventures.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eventures.Areas.Identity.Pages.Account;

namespace Eventures.MappingConfiguration
{
    public class EventuresProfile : Profile
    {
        public EventuresProfile()
        {
            this.CreateMap<RegisterModel.InputModel, EventuresUser>();
            this.CreateMap<Event, EventViewModel>();
            this.CreateMap<Order, OrderEventViewModel>();
            this.CreateMap<Order, AllOrderViewModel>();


            //this.CreateMap<IList<EventViewModel>, IList<Event>>();
            //this.CreateMap<Event, EventViewModel>();
        }
    }
}
