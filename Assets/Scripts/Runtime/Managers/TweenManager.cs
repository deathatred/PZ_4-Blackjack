using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class TweenManager : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    private void OnSceneUnloaded(Scene scene)
    {
        DOTween.KillAll();
    }
}
