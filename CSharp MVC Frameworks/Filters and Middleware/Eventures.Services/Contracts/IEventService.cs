using Eventures.Models;
using Eventures.Models.BindingModels;

namespace Eventures.Services.Contracts
{
    public interface IEventService
    {
        void Create(CreateEvetBindingModel model);

        Event[] GetAllEvents();
    }
}
