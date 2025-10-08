using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameHistoryScrollView : MonoBehaviour
{
    [SerializeField] private RectTransform _content;
    [SerializeField] private RoundHistoryTabSingle _roundHistoryTabSingle;

    [Inject] private EventBus _eventBus;
    [Inject] private SaveManager _saveManager;
    [Inject] private FirebaseManager _firebaseManager;

    private void Start()
    {
        InitAsync().Forget();
        SubscribeToEvents();
    }
    private void OnDisable()
    {
        UnSubscribeFromEvents();
    }

    private async UniTask InitAsync()
    {
        float timeout = 10f;
        float timer = 0f;

        while (!_firebaseManager.IsReady)
        {
            await UniTask.Yield(); 
            timer += Time.deltaTime;

            if (timer >= timeout)
            {
                return; 
            }
        }
        List<GameResult> results = await _firebaseManager.LoadAllGamesDataAsync();

        for (int i = results.Count - 1; i >= 0; i--)
        {
            GameResult gameResult = results[i];
            RoundHistoryTabSingle roundHistoryTabSingle = Instantiate(_roundHistoryTabSingle, _content);
            roundHistoryTabSingle.Init(gameResult);
        }
    }
    private void SubscribeToEvents()
    {
        _eventBus.Subscribe<RoundEndedEvent>(AddHistoryTabSingle);
    }
    private void UnSubscribeFromEvents()
    {
        _eventBus.Unsubscribe<RoundEndedEvent>(AddHistoryTabSingle);
    }
    private void AddHistoryTabSingle(RoundEndedEvent e)
    {
        RoundHistoryTabSingle tabSingle = Instantiate(_roundHistoryTabSingle, _content);
        tabSingle.transform.SetSiblingIndex(0);
        tabSingle.Init(e.GameResult);
    }
}
