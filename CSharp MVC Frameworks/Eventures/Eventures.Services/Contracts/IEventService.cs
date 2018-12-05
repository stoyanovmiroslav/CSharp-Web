using Eventures.Models;
using Eventures.Models.BindingModels;
using System.Collections.Generic;

namespace Eventures.Services.Contracts
{
    public interface IEventService
    {
        void Create(CreateEvetBindingModel model);

        IList<Event> GetAllAvailableEvents();

        int GetAvailableTickets(string id);
    }
}