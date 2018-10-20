using System;
using System.Linq;

namespace SIS.MvcFramework
{
    public static class ObjectMapper
    {
        public static T To<T>(this object source)
            where T: new()
        {
            var destination = new T();

            var sourceParameters = source.GetType().GetProperties();
            var destinationParameters = destination.GetType().GetProperties();

            foreach (var sourceParameter in sourceParameters)
            {
                var property = destinationParameters.FirstOrDefault(x => 
                                            x.Name.ToLower() == sourceParameter.Name.ToLower());

                if (property == null || !property.SetMethod.IsPublic 
                                     || !sourceParameter.GetMethod.IsPublic)
                {
                    continue;
                }

                var value = sourceParameter.GetValue(source);

                var convertedValue = Convert.ChangeType(value, property.PropertyType);
                property.SetValue(destination, convertedValue);
            }

            return destination;
        }
    }
}