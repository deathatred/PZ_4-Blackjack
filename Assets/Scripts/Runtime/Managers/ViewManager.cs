using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class ViewManager: IDisposable
{
    private const int GAME_VIEW_ID = 0;
    private const int GAME_END_VIEW_ID = 1;
    private const int MENU_VIEW_ID = 2;
    private const int HISTORY_VIEW_ID = 3;

    private readonly List<Canvas> _views;
    private readonly EventBus _eventBus;

    public ViewManager(List<Canvas> canvases, EventBus eventBus)
    {
        _views = new List<Canvas>();
        _views = canvases;
        _eventBus = eventBus;
        SubscribeToEvents();
        ChangeCanvas(MENU_VIEW_ID);
    }
    public void ChangeCanvas(int id)
    {
        if (id >= _views.Count)
        {
            return;
        }
        foreach (Canvas canvas in _views)
        {
            if (canvas != null)
                canvas.enabled = false;
        }
        
        if(_views[id] != null)
            _views[id].enabled = true;
    }
    private void SubscribeToEvents()
    {
        _eventBus.Subscribe<PlayPressedEvent>(PlayPressed);
        _eventBus.Subscribe<TryAgainPressedEvent>(TryAgainPressed);
        _eventBus.Subscribe<GameOverEvent>(GameOverDel);
        _eventBus.Subscribe<HistoryPressedEvent>(HistoryPressed);
        _eventBus.Subscribe<BackPressedEvent>(BackPressed);
        _eventBus.Subscribe<MenuButtonPressedEvent>(MenuPressed);
    }
    private void UnsubscribeFromEvents()
    {
        _eventBus.Unsubscribe<PlayPressedEvent>(PlayPressed);
        _eventBus.Unsubscribe<TryAgainPressedEvent>(TryAgainPressed);
        _eventBus.Unsubscribe<GameOverEvent>(GameOverDel);
        _eventBus.Unsubscribe<HistoryPressedEvent>(HistoryPressed);
        _eventBus.Unsubscribe<BackPressedEvent>(BackPressed);
        _eventBus.Unsubscribe<MenuButtonPressedEvent>(MenuPressed);
    }
    private void MenuPressed(GameEventBase e) => ChangeCanvas(MENU_VIEW_ID);
    private void PlayPressed(GameEventBase e)
    {
        ChangeCanvas(GAME_VIEW_ID);
    }
    private void HistoryPressed(GameEventBase e)
    {
        ChangeCanvas(HISTORY_VIEW_ID);
    }
    private void BackPressed(GameEventBase e)
    {
        ChangeCanvas(MENU_VIEW_ID);
    }
    private void TryAgainPressed(GameEventBase e)
    {
        Dispose();
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
    private void GameOverDel(GameEventBase e)
    {
        ChangeCanvas(GAME_END_VIEW_ID);
    }

    public void Dispose()
    {
        UnsubscribeFromEvents();
        _views.Clear();
    }
}