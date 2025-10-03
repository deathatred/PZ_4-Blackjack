using System;
using UnityEngine;

public class MoneyManager : IDisposable
{
    private readonly EventBus _eventBus;
    public int MoneyAmount { get; private set; } = 0;
    private int _currentBet = 0;
    public MoneyManager(EventBus eventBus)
    {
        _eventBus = eventBus;
        MoneyAmount = 1000;
        _eventBus.Subscribe<DealerWinEvent>(DealerWin);
        _eventBus.Subscribe<PlayerWinEvent>(DealerLost);
    }

    public void PlaceBet(int bet)
    {
        _currentBet = bet;
        MoneyAmount -= bet;
        _eventBus.Publish(new BettingEndedEvent());
    }
    public void PlayerWin()
    {
        MoneyAmount += _currentBet * 2;
        _currentBet = 0;
        _eventBus.Publish(new MoneyChangedEvent(MoneyAmount));
    }
    public void PlayerLost()
    {
        _currentBet = 0;
    }
    private void DealerWin(GameEventBase e)
    {
        PlayerLost();
    }
    private void DealerLost(GameEventBase e)
    {
        PlayerWin();
    }

    public void Dispose()
    {
        _eventBus.Unsubscribe<DealerWinEvent>(DealerWin);
        _eventBus.Unsubscribe<PlayerWinEvent>(DealerLost);
    }
}
