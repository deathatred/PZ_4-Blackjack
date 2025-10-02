using UnityEngine;

public class PlayerDrawnCardEvent : GameEventBase
{
  public int Score { get; private set; }
    public PlayerDrawnCardEvent(int score)
    {
        Score = score;  
    }
}
