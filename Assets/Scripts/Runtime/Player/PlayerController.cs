using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerController : MonoBehaviour
{
    
    [SerializeField] private Transform handAnchor;
    private float spacing = 2f;
    private DeckManager _deckManager;
    private EventBus _eventBus;
    public Hand Hand { get; private set; }

    [Inject]
    public void Construct(DeckManager deck, EventBus eventBus)
    {
        _deckManager = deck;
        Hand= new Hand(_deckManager);
        _eventBus =  eventBus;
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
            _eventBus.Publish(new PlayerDrawnCardEvent(Hand.CalculateScore()));
        }
        if (Hand.CalculateScore() == 21) 
        {
            _eventBus.Publish(new PlayerBlackjackEvent());
        }
        else
        {
            _eventBus.Publish(new DealingFinishedEvent());
        }
    }
    private async UniTask TakeCardAsync()
    {
        Card card = _deckManager.DrawCard();
        if (card == null) { Debug.Log(_deckManager.GetDeckCount() + "count"); return; }
        _eventBus.Publish(new DealingCardsToPlayerStartedEvent());
        Hand.AddCard(card);
        await card.DrawFromDeck(handAnchor.position);
        Hand.UpdateHandLayout(handAnchor, spacing);
        _eventBus.Publish(new PlayerDrawnCardEvent(Hand.CalculateScore()));
        _eventBus.Publish(new DealingCardsToPlayerEndedEvent());
    }
    private async UniTask TakeCardTurn()
    {
        await TakeCardAsync();
        if (Hand.CalculateScore() == 21)
        {
            _eventBus.Publish(new PlayerBlackjackEvent());
        }
        if (Hand.CalculateScore() > 21)
        {
            print("Player lost");
            _eventBus.Publish(new DealerWinEvent());
        }
    }
    private void SubscribeToEvents()
    {
        _eventBus.Subscribe<DealingStartedEvent>(FirstTurn);
        _eventBus.Subscribe<TakeButtonPressedEvent>(TakeTurn);
        _eventBus.Subscribe<EnteredResetStateEvent>(Hand.ReturnCards);
    }
    private void UnsubscribeFromEvents()
    {
        _eventBus.Unsubscribe<DealingStartedEvent>(FirstTurn);
        _eventBus.Unsubscribe<TakeButtonPressedEvent>(TakeTurn);
        _eventBus.Unsubscribe<EnteredResetStateEvent>(Hand.ReturnCards);
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
