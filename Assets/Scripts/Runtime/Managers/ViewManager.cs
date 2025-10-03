using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class ViewManager : IDisposable
{
    private const int GAME_VIEW_ID = 0;
    private const int GAME_END_VIEW_ID = 1;
    private const int MENU_VIEW_ID = 2;


    private List<Canvas> _views = new();

    public ViewManager(List<Canvas> canvases)
    {
        Debug.Log("New ViewManager created with canvases: " + _views.Count);
        _views = canvases;
        foreach (var view in _views)
        {
            Debug.Log(view.ToString());
        }
        ChangeCanvas(MENU_VIEW_ID);
        SubscribeToEvents();
    }
    public void ChangeCanvas(int id)
    {
        if (id > _views.Count)
        {
            Debug.LogError($"This canvas id {id}, does not exist");
            return;
        }
        foreach (Canvas canvas in _views)
        {
            canvas.enabled = false;
        }
        _views[id].enabled = true;
    }
    private void SubscribeToEvents()
    {
        EventBus.Subscribe<PlayPressedEvent>(PlayPressed);
        EventBus.Subscribe<TryAgainPressedEvent>(TryAgainPressed);
        EventBus.Subscribe<GameOverEvent>(GameOverDel);
    }
    private void UnsubscribeFromEvents()
    {
        Debug.Log("DispoceCalled");
        EventBus.Unsubscribe<PlayPressedEvent>(PlayPressed);
        EventBus.Unsubscribe<TryAgainPressedEvent>(TryAgainPressed);
        EventBus.Unsubscribe<GameOverEvent>(GameOverDel);
    }
    private void PlayPressed(GameEventBase e)
    {
        ChangeCanvas(GAME_VIEW_ID);
    }
    private void TryAgainPressed(GameEventBase e)
    {
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