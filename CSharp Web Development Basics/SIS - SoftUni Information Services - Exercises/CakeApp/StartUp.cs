using SIS.Framework.Api;
using SIS.Framework.Services;
using SIS.Framework.Services.Contracts;

namespace CakeApp
{
    public class StartUp : IMvcApplication
    {
        public void Configure()
        {
        }

        public void ConfigureService(IServiceCollection serviceCollection)
        {
            serviceCollection.AddService<IHashService, HashService>();
        }
    }
}
