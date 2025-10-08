using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

public class Hand
{
    private List<Card> _cards = new();
    private DeckManager _deckManager;
    private CancellationTokenSource _cts = new CancellationTokenSource();
    public Hand(DeckManager deckManager)
    {
        _deckManager = deckManager;
    }
    public void AddCard(Card card)
    {
        _cards.Add(card);
    }
    public void RemoveCard(Card card)
    {
        _cards.Remove(card);
    }
    public int CalculateScore()
    {
        int score = 0;
        int aces = 0;

        foreach (var card in _cards)
        {
            score += card.GetValue();
            if (card.GetValue() == 11) aces++;
        }
        while (score > 21 && aces > 0)
        {
            score -= 10;
            aces--;
        }

        return score;
    }
    public void UpdateHandLayout(Transform handAnchor,float spacing)
    {
        List<Vector3> positions = LayoutManager.GetLinearLayout(_cards.Count, spacing);
        for (int i = 0; i < _cards.Count; i++)
        {
            Vector3 targetPos = handAnchor.position + positions[i];
            _cards[i].Move(targetPos, i,_cts).Forget();
        }
    }
    public void ReturnCards(GameEventBase e)
    {
        foreach (var card in GetCards())
        {
            card.ReturnToDeck(_cts).Forget();
            _deckManager.ReturnCard(card);
        }
        _cards.Clear();
    }
    public IReadOnlyList<Card> GetCards() => _cards;
}
