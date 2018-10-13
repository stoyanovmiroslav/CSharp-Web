using SIS.MvcFramework.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIS.MvcFramework.Services
{
    public class ServiceCollection : IServiceCollection
    {
        private Dictionary<Type, Type> dependencyContainer;

        public ServiceCollection()
        {
            this.dependencyContainer = new Dictionary<Type, Type>();
        }

        public void AddService<TSource, TDestination>()
        {
            this.dependencyContainer[typeof(TSource)] = typeof(TDestination);
        }

        public T CreateInstance<T>()
        {
            return (T)this.CreateInstance(typeof(T));
        }

        public object CreateInstance(Type type)
        {
            Type destinationType = null;

            if (type.IsClass)
            {
                destinationType = type;
            }
            else if (!this.dependencyContainer.ContainsKey(type))
            {
                throw new InvalidOperationException($"There is no such service: {type.Name}");
            }
            else
            {
                destinationType = this.dependencyContainer[type];
            }

            var constructorParameters = destinationType.GetConstructors().First().GetParameters();

            var parameters = new List<object>();

            foreach (var constructorParameter in constructorParameters)
            {
                var parameterType = constructorParameter.ParameterType;

                parameters.Add(this.CreateInstance(parameterType));
            }

            var instance = Activator.CreateInstance(destinationType, parameters.ToArray());

            return instance;
        }
    }
}
