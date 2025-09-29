using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardDatabase", menuName = "CardDatabase")]
public class CardDatabase : ScriptableObject
{
    public List<CardData> CardsList;
}
