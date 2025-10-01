using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ViewManager
{
    private const int GAME_VIEW_ID = 0;
    private const int GAME_END_VIEW_ID = 1;
    private const int MENU_VIEW_ID = 2;

    private readonly List<Canvas> _views = new();
  
    public ViewManager(List<Canvas> canvases)
    {
        _views = canvases;
        ChangeCanvas(0);
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

    }
}
