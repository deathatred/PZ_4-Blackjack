using UnityEngine;

public class RoundEndedEvent : GameEventBase
{
    public GameResult GameResult { get; private set; }
    public int MoneyAmount { get; private set; }
    public RoundEndedEvent(int moneyAmount,GameResult gameResult)
    {
        this.MoneyAmount = moneyAmount;
        this.GameResult = gameResult;
    }
}
