using System;
using System.Collections.Generic;
using ServiceLocator.Interfaces;

namespace MicroondasProject
{
    public static class ServiceLocator
    {
        private static Dictionary<object, object> services = null;

        public static T Get<T>() where T : IServiceLocator
        {
            if (services != null)
            {
                object service;
                if (services.TryGetValue(typeof(T), out service))
                    return (T)service;
            }

            return default(T);
        }

        public static void Set<T>(T service) where T : IServiceLocator
        {
            if (services == null)
                services = new Dictionary<object, object>();

            if (Get<T>() == null)
                services.Add(typeof(T), service);
            else
                throw new Exception("Serviço já registrado.");
        }
    }
}
