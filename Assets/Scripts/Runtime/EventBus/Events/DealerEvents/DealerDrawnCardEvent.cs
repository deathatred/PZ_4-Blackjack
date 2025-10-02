using UnityEngine;

public class DealerDrawnCardEvent : GameEventBase
{
    public int Score { get; private set; }
    public DealerDrawnCardEvent(int score)
    {
        Score = score;
    }
}
