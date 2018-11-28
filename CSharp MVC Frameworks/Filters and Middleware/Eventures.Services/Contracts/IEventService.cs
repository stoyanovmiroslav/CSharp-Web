using Eventures.Models;
using Eventures.Models.BindingModels;
using System.Collections.Generic;

namespace Eventures.Services.Contracts
{
    public interface IEventService
    {
        void Create(CreateEvetBindingModel model);

        IList<Event> GetAllEvents();
    }
}