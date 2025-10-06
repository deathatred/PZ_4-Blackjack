using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Zenject;

public class DeckManager : MonoBehaviour, IDisposable
{
    [SerializeField] private CardDatabaseSO _cardDatabaseSO;
    [SerializeField] private GameObject _cardBlankPrefab;
    private Vector3 _defaultCardPos = new Vector3(3f, 0f, 7.5f);    
    private Quaternion _defaultCardRot = Quaternion.Euler(-90f,-90f,-90f);

    private List<Card> _currentDeck = new List<Card>();
    private EventBus _eventBus;

    [Inject]
    public void Construct(EventBus eventBus)
    {
        _eventBus = eventBus;
    }
    [Inject]
    public void Initialize()
    {
        Init(); 
    }

    public void Init()
    {
        foreach (var card in _cardDatabaseSO.CardsList)
        {
            GameObject cardGO = Instantiate(_cardBlankPrefab, _defaultCardPos, _defaultCardRot);
            Card newCard = cardGO.GetComponent<Card>();
            _currentDeck.Add(newCard);
            newCard.Setup(card, this);
        }
        Shuffle(_currentDeck);
        SubscribeToEvents();
    }
    private void Shuffle(List<Card> deck)
    {
        for (int i = 0; i < deck.Count; i++)
        {
            int rnd = UnityEngine.Random.Range(i, deck.Count);
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
    public void ReturnCard(Card card)
    {
        print("Card returned");
        _currentDeck.Add(card);
    }
    public int GetDeckCount()
    {
        return _currentDeck.Count;
    }
    public Vector3 GetDeckPosition()
    {
        return _defaultCardPos;
    }
    private void SubscribeToEvents()
    {
        _eventBus.Subscribe<EnteredResetStateEvent>(ShuffleEvent);
    }
    private void UnsubscribeFromEvents()
    {
        _eventBus.Unsubscribe<EnteredResetStateEvent>(ShuffleEvent);
    }
    private void ShuffleEvent(GameEventBase e)
    {
        Shuffle(_currentDeck);
    }

    public void Dispose()
    {
        UnsubscribeFromEvents();
    }
}
