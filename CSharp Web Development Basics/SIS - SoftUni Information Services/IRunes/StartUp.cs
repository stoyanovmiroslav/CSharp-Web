using SIS.MvcFramework.Contracts;
using SIS.MvcFramework.Services;
using SIS.MvcFramework.Services.Contracts;

namespace IRunes
{
    public class StartUp : IMvcApplication
    {
        public void Configure()
        {
            
        }
        
        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddService<IHashService, HashService>();
            serviceCollection.AddService<IUserCookieService, UserCookieService>();
        }
    }
}