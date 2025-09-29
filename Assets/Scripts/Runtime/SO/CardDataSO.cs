using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "CardData")]
public class CardDataSO : ScriptableObject
{
    public CardSuit CardSuit;
    public CardRank CardRank;
    public int CardValue;
    public Sprite FrontImage;
    public Sprite BackImage;
}
