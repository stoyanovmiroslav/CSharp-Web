namespace SIS.MvcFramework.Contracts
{
    public interface IMvcApplication
    {
        void Configure();

        void ConfigureServices();
    }
}