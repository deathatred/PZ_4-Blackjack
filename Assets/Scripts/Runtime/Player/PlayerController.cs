using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform handAnchor;
     private float spacing = 2f;

    private List<Card> _currentCards = new List<Card>();

    [Inject] private DeckManager _deckManager;

    public async UniTask TakeCard()
    {
        Card card = _deckManager.DrawCard();
        _currentCards.Add(card);
        await card.DrawFromDeck(handAnchor.position);
        UpdateHandLayout();
        print(_currentCards[_currentCards.Count - 1].GetName());
    }
    private void UpdateHandLayout()
    {
        List<Vector3> positions = LayoutManager.GetLinearLayout(_currentCards.Count, spacing);
        for (int i = 0; i < _currentCards.Count; i++)
        {
            Vector3 targetPos = handAnchor.position + positions[i];
            _currentCards[i].Move(targetPos,i).Forget(); 
        }
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
