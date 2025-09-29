using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Dealer : MonoBehaviour
{
    private List<Card> _currentCards = new List<Card>();
    [Inject] private DeckManager _deckManager;
    public void TakeCard()
    {
        _currentCards.Add(_deckManager.DrawCard());
        print(_currentCards[_currentCards.Count - 1].GetName());
    }
    public int CalculateScore()
    {
        int score = 0;
        int aces = 0;

        foreach (var card in _currentCards)
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
}
