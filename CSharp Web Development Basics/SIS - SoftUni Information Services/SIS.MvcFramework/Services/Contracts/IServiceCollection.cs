using System;

namespace SIS.MvcFramework.Services.Contracts
{
    public interface IServiceCollection
    {
        void AddService<TSource, TDestination>();

        T CreateInstance<T>();

        object CreateInstance(Type type);
    }
}