using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Cysharp.Threading.Tasks;

public class Dealer : MonoBehaviour
{
    public Hand Hand { get; private set; } = new Hand();
    [SerializeField] private Transform handAnchor;
    private float spacing = 2f;
    [Inject] private DeckManager _deckManager;

    public void OnEnable()
    {
        EventBus.Subscribe<DealingStartedEvent>(FirstTurn);
        EventBus.Subscribe<DealerTurnStartedEvent>(StartDealerTurn);
    }
    private void OnDisable()
    {
        EventBus.Unsubscribe<DealingStartedEvent>(FirstTurn);
        EventBus.Unsubscribe<DealerTurnStartedEvent>(StartDealerTurn);
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
            }
                Hand.UpdateHandLayout(handAnchor, spacing);
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
        await Hand.GetCards()[1].Flip();
        while (Hand.CalculateScore() < 17)
        {
           await DrawCardAsync();
        }
        if (Hand.CalculateScore() > 17 && Hand.CalculateScore() <= 21)
        {
            EventBus.Publish(new DealerTurnEndedEvent());
        }
        else
        {
            print("dealer lost, player wins ");
            //dealer lost, player wins  
        }
    }
}
