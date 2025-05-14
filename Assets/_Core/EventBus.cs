using System;
using System.Collections.Generic;

/// <summary>
/// Simple in-game event bus for decoupled messaging.
/// </summary>
public static class EventBus
{
    private static readonly Dictionary<string, Action<object>> eventDict = new();

    /// <summary>
    /// Subscribe a callback to an event key.
/// </summary>
    public static void Subscribe<T>(string key, Action<T> callback)
    {
        if (!eventDict.ContainsKey(key))
            eventDict[key] = _ => { };
        eventDict[key] += payload => callback((T)payload);
    }

    /// <summary>
    /// Publish an event with a payload to all subscribers.
/// </summary>
    public static void Publish(string key, object payload)
    {
        if (eventDict.TryGetValue(key, out var action))
            action(payload);
    }
}
