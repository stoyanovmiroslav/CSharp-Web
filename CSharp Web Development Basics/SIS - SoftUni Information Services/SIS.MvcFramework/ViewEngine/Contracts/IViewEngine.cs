namespace SIS.MvcFramework.ViewEngine.Contracts
{
    public interface IViewEngine
    {
        string GetHtml<T>(string viewName, string viewCode, T model, string user);
    }
}