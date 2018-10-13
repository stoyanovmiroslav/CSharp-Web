namespace SIS.MvcFramework.Services.Contracts
{
    public interface IHashService
    {
        string Hash(string stringToHash);

        string StrongHash(string stringToHash);
    }
}