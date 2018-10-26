using SIS.MvcFramework.ViewEngine;

namespace SIS.MvcFramework.ViewEngine.Contracts
{
    public interface IView<T>
    {
        string GetHtml(T model, UserModel name);
    }
}