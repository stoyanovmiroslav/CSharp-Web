using System;

namespace SIS.Framework.Services.Contracts
{
    public interface IServiceCollection
    {
        void AddService<TSource, TDestination>();

        T CreateInstance<T>();

        object CreateInstance(Type type);
    }
}