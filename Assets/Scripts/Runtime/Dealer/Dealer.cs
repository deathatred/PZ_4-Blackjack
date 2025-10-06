using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Cysharp.Threading.Tasks;

public class Dealer : MonoBehaviour
{
    public Hand Hand { get; private set; }
    [SerializeField] private Transform handAnchor;
    private float spacing = 2f;
    private DeckManager _deckManager;
    private EventBus _eventBus;
    
    [Inject]
    public void Construct(DeckManager deck, EventBus eventBus)
    {
        _deckManager = deck;
        Hand = new Hand(_deckManager);
        _eventBus = eventBus;
    }

    public void OnEnable()
    {
        _eventBus.Subscribe<DealingStartedEvent>(FirstTurn);
        _eventBus.Subscribe<DealerTurnStartedEvent>(StartDealerTurn);
        _eventBus.Subscribe<EnteredResetStateEvent>(Hand.ReturnCards);
    }
    private void OnDisable()
    {
        _eventBus.Unsubscribe<DealingStartedEvent>(FirstTurn);
        _eventBus.Unsubscribe<DealerTurnStartedEvent>(StartDealerTurn);
        _eventBus.Unsubscribe<EnteredResetStateEvent>(Hand.ReturnCards);
    }

    public void FirstTurn(DealingStartedEvent e)
    {
        TakeFirstTurnAsync().Forget();
    }
    public async UniTask TakeFirstTurnAsync()
    {
        for (int i = 0; i < 2; i++)
        {
            Card card = _deckManager.DrawCard();
            Hand.AddCard(card);
            if (i == 1)
            {
                await card.DrawFromDeck(handAnchor.position, false);
             
            }
            else
            {
                await card.DrawFromDeck(handAnchor.position);
                _eventBus.Publish(new DealerDrawnCardEvent(Hand.CalculateScore()));
            }
                Hand.UpdateHandLayout(handAnchor, spacing);
        }
    }
    public async UniTask ShowCard()
    {
        if (!Hand.GetCards()[1].IsFlipped)
        {  
            await Hand.GetCards()[1].Flip();
            _eventBus.Publish(new DealerDrawnCardEvent(Hand.CalculateScore()));
        }
    }
    public async UniTask DrawCardAsync()
    {
        if (Hand.CalculateScore() < 17)
        {
            Card card = _deckManager.DrawCard();
            Hand.AddCard(card);
            await card.DrawFromDeck(handAnchor.position);
            Hand.UpdateHandLayout(handAnchor, spacing);
        }
    }
    private void StartDealerTurn(GameEventBase e)
    {
        DealerTurn().Forget();
    }
    private async UniTask DealerTurn()
    {
        await ShowCard();
        while (Hand.CalculateScore() < 17)
        {
           await DrawCardAsync();
           _eventBus.Publish(new DealerDrawnCardEvent(Hand.CalculateScore()));
        }
        if (Hand.CalculateScore() >= 17 && Hand.CalculateScore() <= 21)
        {
            _eventBus.Publish(new DealerTurnEndedEvent());
        }
        else
        {
            print("dealer lost, player wins ");
            _eventBus.Publish(new DealerOverflowEvent());
        }
    }
}
