using System;
using System.Collections.Generic;
using UnityEngine;

public static class EventBus
{
    private static readonly Dictionary<Type, List<Action<GameEventBase>>> _subscribers =
        new Dictionary<Type, List<Action<GameEventBase>>>();
    private static readonly Dictionary<Delegate, Action<GameEventBase>> _wrappers = 
        new Dictionary<Delegate, Action<GameEventBase>>();

    public static void Subscribe<T>(Action<T> action) where T : GameEventBase
    {
        var type = typeof(T);
        if (!_subscribers.ContainsKey(type))
        {
            _subscribers[type] = new List<Action<GameEventBase>>();
        }

        Action<GameEventBase> wrapper = (e) => action((T)e);

        _subscribers[type].Add(wrapper);
        _wrappers[action] = wrapper;
    }
    public static void Unsubscribe<T>(Action<T> action) where T : GameEventBase
    {
        var type = typeof(T);
        if (!_subscribers.ContainsKey(type))
        {
            return;
        }
        _subscribers[type].Remove(_wrappers[action]);
        _wrappers.Remove(action);
    }
    public static void Publish(GameEventBase action)
    {
        var type = action.GetType();
        if (!_subscribers.ContainsKey(type))
        {
            return;
        }
        foreach (var subscriber in _subscribers[type])
        {
            subscriber.Invoke(action);
        }
    }
}
