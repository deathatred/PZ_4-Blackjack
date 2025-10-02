using System;
using UnityEngine;

public class MoneyManager : IDisposable
{
    public int MoneyAmount { get; private set; } = 0;
    private int _currentBet = 0;
    public MoneyManager()
    {
        MoneyAmount = 1000;
        EventBus.Subscribe<DealerWinEvent>(DealerWin);
        EventBus.Subscribe<PlayerWinEvent>(DealerLost);
    }

    public void PlaceBet(int bet)
    {
        _currentBet = bet;
        MoneyAmount -= bet;
        EventBus.Publish(new BettingEndedEvent());
    }
    public void PlayerWin()
    {
        MoneyAmount += _currentBet * 2;
        _currentBet = 0;
        EventBus.Publish(new MoneyChangedEvent(MoneyAmount));
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
        EventBus.Unsubscribe<DealerWinEvent>(DealerWin);
        EventBus.Unsubscribe<PlayerWinEvent>(DealerLost);
    }
}
