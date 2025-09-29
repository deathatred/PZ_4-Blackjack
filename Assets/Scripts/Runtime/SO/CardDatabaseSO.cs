using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardDatabase", menuName = "CardDatabase")]
public class CardDatabaseSO : ScriptableObject
{
    public List<CardDataSO> CardsList;
}
