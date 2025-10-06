using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using Zenject;

public class MoneyManager : IDisposable
{
    private readonly EventBus _eventBus;
    private readonly FirebaseManager _firebaseManager;
    public int MoneyAmount { get; private set; } = 0;
    private int _currentBet = 0;
    public MoneyManager(EventBus eventBus, FirebaseManager firebaseManager)
    {
        _eventBus = eventBus;
        _firebaseManager = firebaseManager;
        _eventBus.Subscribe<DealerWinEvent>(DealerWin);
        _eventBus.Subscribe<PlayerWinEvent>(DealerLost);
        _eventBus.Subscribe<DrawEvent>(GameDraw);
    }
    [Inject]
    public void Initialize()
    {
        InitializeAsync().Forget();
    }
    //public async UniTask InitializeAsync()
    //{
    //    try
    //    {
    //        bool isReady = await UniTask.WaitUntil(() => _firebaseManager.IsReady)
    //        .Timeout(TimeSpan.FromSeconds(10))
    //        .SuppressCancellationThrow();
    //        if (!isReady)
    //        {
    //            Debug.LogWarning("Firebase did not initialize in time, using PlayerPrefs fallback.");
    //            MoneyAmount = PlayerPrefs.GetInt("MoneyAmount", 1000);
    //            _eventBus.Publish(new MoneyChangedEvent(MoneyAmount));
    //            return;
    //        }
    //        MoneyAmount = await _firebaseManager.LoadMoneyAmountAsync();
    //        if (MoneyAmount <= 0)
    //            MoneyAmount = PlayerPrefs.GetInt("MoneyAmount", 1000);

    //        if (MoneyAmount <= 0) { MoneyAmount = PlayerPrefs.GetInt("MoneyAmount", 1000); }
    //    }
    //    catch (Exception ex)
    //    {
    //        Debug.LogWarning($"MoneyManager Init failed. Reason: {ex}");
    //        MoneyAmount = PlayerPrefs.GetInt("MoneyAmount", 1000);
    //    }

    //    _eventBus.Publish(new MoneyChangedEvent(MoneyAmount));
    //}

    public async UniTask InitializeAsync()
    {
        try
        {
            var task = await UniTask.WhenAny(
              UniTask.WaitUntil(() => _firebaseManager.IsReady),
              UniTask.Delay(TimeSpan.FromSeconds(10))
            );

            if (!_firebaseManager.IsReady)
            {
                Debug.LogWarning("Firebase did not initialize in time");
                MoneyAmount = PlayerPrefs.GetInt("MoneyAmount", 1000);
            }
            else
            {
                MoneyAmount = await _firebaseManager.LoadMoneyAmountAsync();
                if (MoneyAmount <= 0)
                    MoneyAmount = PlayerPrefs.GetInt("MoneyAmount", 1000);
            }
            Debug.Log($"Money loaded: {MoneyAmount}");
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"MoneyManager Init failed Reason: {ex.Message}");
            MoneyAmount = PlayerPrefs.GetInt("MoneyAmount", 1000);
        }

        _eventBus.Publish(new MoneyChangedEvent(MoneyAmount));
    }

    public void PlaceBet(int bet)
    {
        _currentBet = bet;
        MoneyAmount -= bet;
        _eventBus.Publish(new BettingEndedEvent());
    }
    private void PlayerWin()
    {
        MoneyAmount += _currentBet * 2;
        Debug.Log(_currentBet + "Current bet");
        PublishResult(+_currentBet, GameOutcome.Win);
        _currentBet = 0;
        _eventBus.Publish(new MoneyChangedEvent(MoneyAmount));
    }
    private void PlayerLost()
    {
        PublishResult(-_currentBet, GameOutcome.Lose);
        _currentBet = 0;
    }
    private void Draw()
    {
        MoneyAmount += _currentBet;
        _currentBet = 0;
        _eventBus.Publish(new MoneyChangedEvent(MoneyAmount));
        PublishResult(0, GameOutcome.Draw);
    }
    private void DealerWin(GameEventBase e)
    {
        Debug.Log("MoneyManager DealerWin called");
        PlayerLost();
    }
    private void DealerLost(GameEventBase e)
    {
        PlayerWin();
    }
    private void GameDraw(GameEventBase e)
    {
        Draw();
    }

    private void PublishResult(int moneyDelta, GameOutcome outcome)
    {
        int roundNumber = GameResultStorage.GetGameCount() + 1;
        var result = new GameResult
        {
            RoundNumber = roundNumber,
            Result = outcome.ToString(),
            MoneyChange = moneyDelta
        };
        GameResultStorage.SaveGameResult(MoneyAmount, result);

        _currentBet = 0;
        _eventBus.Publish(new RoundEndedEvent(MoneyAmount, result));
    }
    public void Dispose()
    {
        _eventBus.Unsubscribe<DealerWinEvent>(DealerWin);
        _eventBus.Unsubscribe<PlayerWinEvent>(DealerLost);
        _eventBus.Unsubscribe<DrawEvent>(GameDraw);
    }
}
