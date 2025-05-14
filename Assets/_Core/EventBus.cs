using System;
using System.Collections.Generic;

public static class EventBus
{
    private static readonly Dictionary<string, Action<object>> eventDict = new Dictionary<string, Action<object>>();

    public static void Subscribe<T>(string key, Action<T> callback)
    {
        if (!eventDict.ContainsKey(key))
            eventDict[key] = _ => { };
        eventDict[key] += payload => callback((T)payload);
    }

    public static void Publish(string key, object payload)
    {
        if (eventDict.TryGetValue(key, out var action))
            action(payload);
    }
}
