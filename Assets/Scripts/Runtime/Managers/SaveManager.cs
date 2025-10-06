using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using Zenject;

public class SaveManager : IDisposable
{
    [Inject] private EventBus _eventBus;
    [Inject] private FirebaseManager _firebaseManager;
    private int _localRound = 0;

    [Inject]
    public void Initialize()
    {
        Init();
    }
    private void Init()
    {
        _eventBus.Subscribe<RoundEndedEvent>(OnGameEnded);
    }

    private async UniTask OnGameEndedAsync(RoundEndedEvent e)
    {
        var result = e.GameResult;
        _localRound = result.RoundNumber;

        await _firebaseManager.SaveRoundDataAsync(e.MoneyAmount,result);

        string key = $"GameResult_{result.RoundNumber}";
        string json = JsonUtility.ToJson(result);
        PlayerPrefs.SetString(key, json);
        PlayerPrefs.Save();
    }
    private void OnGameEnded(RoundEndedEvent e)
    {
        OnGameEndedAsync(e).Forget();
    }
    public void Dispose()
    {
        _eventBus.Unsubscribe<RoundEndedEvent>(OnGameEnded);
    }
}
