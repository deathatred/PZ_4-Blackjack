using System;
using System.Collections.Generic;
using Zenject;

public class EventBus : IDisposable
{
    private readonly Dictionary<Type, List<Action<GameEventBase>>> _subscribers =
        new Dictionary<Type, List<Action<GameEventBase>>>();
    private Dictionary<Delegate, Action<GameEventBase>> _wrappers = new();
    public void Publish(GameEventBase eventData)
    {
        var type = eventData.GetType();
        if (!_subscribers.TryGetValue(type, out var handlers)) return;
        
        var snapshot = new List<Action<GameEventBase>>(handlers);
        foreach (var handler in snapshot)
        {
            try
            {
                handler.Invoke(eventData);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError($"EventBus handler error: {e}");
            }
        }
    }

  

    public void Subscribe<T>(Action<T> action) where T : GameEventBase
    {
        var type = typeof(T);
        if (!_subscribers.TryGetValue(type, out var list))
        {
            list = new List<Action<GameEventBase>>();
            _subscribers[type] = list;
        }

        Action<GameEventBase> wrapper = e => action((T)e);
        list.Add(wrapper);
        _wrappers[action] = wrapper;
    }

    public void Unsubscribe<T>(Action<T> action) where T : GameEventBase
    {
        if (_wrappers.TryGetValue(action, out var wrapper))
        {
            var type = typeof(T);
            if (_subscribers.TryGetValue(type, out var list))
            {
                list.Remove(wrapper);
            }
            _wrappers.Remove(action);
        }
    }

    public void Clear()
    {
        _subscribers.Clear();
    }
    
    public void Dispose()
    {
        Clear();
    }
}