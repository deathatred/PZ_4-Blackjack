using System;
using System.Collections.Generic;

public class EventBus : IDisposable
{
    private readonly Dictionary<Type, List<Action<GameEventBase>>> _subscribers =
        new Dictionary<Type, List<Action<GameEventBase>>>();

    private readonly Dictionary<Delegate, Action<GameEventBase>> _wrappers =
        new Dictionary<Delegate, Action<GameEventBase>>();

    public void Subscribe<T>(Action<T> action) where T : GameEventBase
    {
        var type = typeof(T);
        if (!_subscribers.ContainsKey(type))
        {
            _subscribers[type] = new List<Action<GameEventBase>>();
        }

        Action<GameEventBase> wrapper = e => action((T)e);
        _subscribers[type].Add(wrapper);
        _wrappers[action] = wrapper;
    }

    public void Unsubscribe<T>(Action<T> action) where T : GameEventBase
    {
        var type = typeof(T);
        if (!_subscribers.ContainsKey(type)) return;

        if (_wrappers.TryGetValue(action, out var wrapper))
        {
            _subscribers[type].Remove(wrapper);
            _wrappers.Remove(action);
        }
    }

    public void Publish(GameEventBase action)
    {
        var type = action.GetType();
        if (!_subscribers.ContainsKey(type)) return;

        var snapshot = new List<Action<GameEventBase>>(_subscribers[type]);
        foreach (var subscriber in snapshot)
        {
            subscriber.Invoke(action);
        }
    }

    public void Clear()
    {
        _subscribers.Clear();
        _wrappers.Clear();
    }
    public void Dispose()
    {
        Clear();
    }
}
