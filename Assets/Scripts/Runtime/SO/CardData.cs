using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "CardData")]
public class CardData : ScriptableObject
{
    public CardSuit CardSuit;
    public CardRank CardRank;
    public int CardValue;
    public Sprite FrontImage;
    public Sprite BackImage;
}
