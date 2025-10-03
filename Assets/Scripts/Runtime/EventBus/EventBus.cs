using System;
using System.Collections.Generic;
using Zenject;

// Безпечний EventBus, DI-сумісний
public class EventBus : IInitializable, IDisposable
{
    // Словник: тип події → список підписників
    private readonly Dictionary<Type, List<Action<GameEventBase>>> _subscribers =
        new Dictionary<Type, List<Action<GameEventBase>>>();

    // Клонування під час Publish для безпечної ітерації
    public void Publish(GameEventBase eventData)
    {
        var type = eventData.GetType();
        if (!_subscribers.TryGetValue(type, out var handlers)) return;

        // Клон списку, щоб безпечно обходитись при видаленні підписників
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

    // Підписка
    public void Subscribe<T>(Action<T> action) where T : GameEventBase
    {
        var type = typeof(T);
        if (!_subscribers.TryGetValue(type, out var list))
        {
            list = new List<Action<GameEventBase>>();
            _subscribers[type] = list;
        }

        // Обгортка для приведення типів
        Action<GameEventBase> wrapper = e => action((T)e);
        list.Add(wrapper);
    }

    // Відписка
    public void Unsubscribe<T>(Action<T> action) where T : GameEventBase
    {
        var type = typeof(T);
        if (!_subscribers.TryGetValue(type, out var list)) return;

        // Видаляємо всі підписники з таким же target+method
        list.RemoveAll(wrapper =>
        {
            var del = wrapper.Target as Delegate;
            return del != null && del.Method == action.Method && del.Target == action.Target;
        });
    }

    // Очистка всіх підписників
    public void Clear()
    {
        _subscribers.Clear();
    }

    // Zenject IInitializable (опційно)
    public void Initialize()
    {
        // Можна робити лог або підписку на глобальні події
    }

    // Zenject IDisposable
    public void Dispose()
    {
        Clear();
    }
}