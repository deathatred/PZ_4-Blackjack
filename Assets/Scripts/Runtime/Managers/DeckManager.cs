using NUnit.Framework;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    [SerializeField] private CardDatabaseSO _cardDatabaseSO;
    [SerializeField] private GameObject _cardBlankPrefab;
    private Vector3 _defaultCardPos = new Vector3(4.5f, 0f, 9f);    
    private Quaternion _defaultCardRot = Quaternion.Euler(-90f,-90f,-90f);

    private List<Card> _currentDeck = new List<Card>();
    public void Init()
    {
        foreach (var card in _cardDatabaseSO.CardsList)
        {
            GameObject cardGO = Instantiate(_cardBlankPrefab, _defaultCardPos, _defaultCardRot);
            Card newCard = cardGO.GetComponent<Card>();
            _currentDeck.Add(newCard);
            newCard.Setup(card);
        }
        Shuffle(_currentDeck);
    }
    private void Shuffle(List<Card> deck)
    {
        for (int i = 0; i < deck.Count; i++)
        {
            int rnd = Random.Range(i, deck.Count);
            var temp = deck[i];
            deck[i] = deck[rnd];
            deck[rnd] = temp;
        }
    }
    public Card DrawCard()
    {
        if (_currentDeck.Count == 0) return null;

        Card card = _currentDeck[0];
        _currentDeck.RemoveAt(0);
        return card;
    }

}
