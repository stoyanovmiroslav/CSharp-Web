using SIS.MvcFramework.Services.Contracts;

namespace SIS.MvcFramework.Contracts
{
    public interface IMvcApplication
    {
        void Configure();

        void ConfigureServices(IServiceCollection serviceCollection);
    }
}