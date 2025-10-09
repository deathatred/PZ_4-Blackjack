using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Zenject;

public class GameHistoryScrollView : MonoBehaviour
{
    [SerializeField] private RectTransform _content;
    [SerializeField] private RoundHistoryTabSingle _roundHistoryTabSingle;

    [Inject] private EventBus _eventBus;
    [Inject] private SaveManager _saveManager;
    [Inject] private FirebaseManager _firebaseManager;

    private CancellationTokenSource _cts = new CancellationTokenSource();

    private void OnEnable()
    {
        _cts = new CancellationTokenSource();
    }
    private void Start()
    {
        InitAsync(_cts).Forget(ex => Debug.LogWarning($"InitAsync failed: {ex.Message}"));
        SubscribeToEvents();
    }
    private void OnDisable()
    {
        UnSubscribeFromEvents();
        _cts.Cancel();
    }

    private async UniTask InitAsync(CancellationTokenSource _cts)
    {
        float timeout = 10f;
        float timer = 0f;

        while (!_firebaseManager.IsReady)
        {
            await UniTask.Yield(_cts.Token); 
            timer += Time.deltaTime;

            if (timer >= timeout)
            {
                return; 
            }
        }
        if (_cts.IsCancellationRequested)
        {
            return;
        }
        var task = _firebaseManager.LoadAllGamesDataAsync();
        List<GameResult> results = await task;
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
