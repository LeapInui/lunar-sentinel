using System.Collections.Generic;
using UnityEngine;

public static class ServiceLocator
{
    public static Dictionary<System.Type, object> services = new Dictionary<System.Type, object>();

    // Registers service
    public static void Register<T>(T service)
    {
        var type = typeof(T);
        if (services.ContainsKey(type)) return;

        services.Add(type, service);
    }
    
    public static T Get<T>()
    {
        var type = typeof(T);

        if (services.ContainsKey(type))
        {
            return (T)services[type];
        }

        return default;
    }
}
