using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerController : MonoBehaviour
{
    
    [SerializeField] private Transform handAnchor;
    private float spacing = 2f;
    private DeckManager _deckManager;
    public Hand Hand { get; private set; }

    [Inject]
    public void Construct(DeckManager deck)
    {
        _deckManager = deck;
        Hand= new Hand(_deckManager);
    }

    private void OnEnable()
    {
        SubscribeToEvents();
    }
    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }
    private async UniTask TakeFirstTurnAsync()
    {
        for (int i = 0; i < 2; i++)
        {
            await TakeCardAsync();
            EventBus.Publish(new PlayerDrawnCardEvent(Hand.CalculateScore()));
        }
        if (Hand.CalculateScore() == 21) 
        {
            EventBus.Publish(new PlayerBlackjackEvent());
        }
        else
        {
            EventBus.Publish(new DealingFinishedEvent());
        }
    }
    private async UniTask TakeCardAsync()
    {
        Card card = _deckManager.DrawCard();
        if (card == null) { Debug.Log(_deckManager.GetDeckCount() + "count"); return; }
        EventBus.Publish(new DealingCardsToPlayerStartedEvent());
        Hand.AddCard(card);
        await card.DrawFromDeck(handAnchor.position);
        Hand.UpdateHandLayout(handAnchor, spacing);
        EventBus.Publish(new PlayerDrawnCardEvent(Hand.CalculateScore()));
        EventBus.Publish(new DealingCardsToPlayerEndedEvent());
    }
    private async UniTask TakeCardTurn()
    {
        await TakeCardAsync();
        if (Hand.CalculateScore() == 21)
        {
            EventBus.Publish(new PlayerBlackjackEvent());
        }
        if (Hand.CalculateScore() > 21)
        {
            print("Player lost");
            EventBus.Publish(new DealerWinEvent());
        }
    }
    private void SubscribeToEvents()
    {
        EventBus.Subscribe<DealingStartedEvent>(FirstTurn);
        EventBus.Subscribe<TakeButtonPressedEvent>(TakeTurn);
        EventBus.Subscribe<EnteredResetStateEvent>(Hand.ReturnCards);
    }
    private void UnsubscribeFromEvents()
    {
        EventBus.Unsubscribe<DealingStartedEvent>(FirstTurn);
        EventBus.Unsubscribe<TakeButtonPressedEvent>(TakeTurn);
        EventBus.Unsubscribe<EnteredResetStateEvent>(Hand.ReturnCards);
    }
  
    private void FirstTurn(DealingStartedEvent e)
    {
        TakeFirstTurnAsync().Forget();
    }
    private void TakeTurn(TakeButtonPressedEvent e)
    {
        TakeCardTurn().Forget();
    }
}
