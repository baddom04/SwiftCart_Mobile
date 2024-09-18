using System;
using System.Collections.Generic;

namespace ShoppingList.Utils
{
    public static class ServiceProvider
    {
        private static readonly Dictionary<Type, object> services = [];

        public static void Register<T>(T service)
        {
            if (service is null) throw new ArgumentNullException(nameof(service));
            services[typeof(T)] = service;
        }

        public static T Resolve<T>()
        {
            return (T)services[typeof(T)];
        }
    }
}
