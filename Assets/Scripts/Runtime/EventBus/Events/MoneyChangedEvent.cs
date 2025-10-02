using UnityEngine;

public class MoneyChangedEvent : GameEventBase
{
    public MoneyChangedEvent(int amount) 
    {
        MoneyAmount = amount;
    }
    public int MoneyAmount { get ; private set; }
}
