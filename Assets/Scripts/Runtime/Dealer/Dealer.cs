using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

public class Dealer : MonoBehaviour
{
    public Hand Hand { get; private set; }
    [SerializeField] private Transform handAnchor;
    private float spacing = 2f;
    private DeckManager _deckManager;
    private EventBus _eventBus;
    private CancellationTokenSource _cts = new CancellationTokenSource();

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
        _cts.Cancel();
    }

    public void FirstTurn(DealingStartedEvent e)
    {
        TakeFirstTurnAsync(_cts).Forget();
    }
    public async UniTask TakeFirstTurnAsync(CancellationTokenSource _cts)
    {
        try
        {
            for (int i = 0; i < 2; i++)
            {
                if (_cts.Token.IsCancellationRequested)
                    return;
                Card card = _deckManager.DrawCard();
                Hand.AddCard(card);
                if (i == 1)
                {
                    await card.DrawFromDeck(handAnchor.position, _cts,false).AttachExternalCancellation(_cts.Token);

                }
                else
                {
                    await card.DrawFromDeck(handAnchor.position, _cts).AttachExternalCancellation(_cts.Token);
                    _eventBus.Publish(new DealerDrawnCardEvent(Hand.CalculateScore()));
                }
                Hand.UpdateHandLayout(handAnchor, spacing);
            }
        }
        catch (OperationCanceledException)
        {
            Debug.Log("First turn cancelled");
        }
    }
    public async UniTask ShowCard(CancellationTokenSource _cts)
    {
        try
        {
            if (!Hand.GetCards()[1].IsFlipped)
            {
                await Hand.GetCards()[1].Flip(_cts);
                if (_cts.Token.IsCancellationRequested)
                {
                    return;
                }
                _eventBus.Publish(new DealerDrawnCardEvent(Hand.CalculateScore()));
            }
        }
        catch (OperationCanceledException)
        {
            Debug.LogWarning("Showing card cancelled properly");
        }
    }
    public async UniTask DrawCardAsync(CancellationTokenSource _cts)
    {
        if (Hand.CalculateScore() < 17)
        {
            if (_cts.Token.IsCancellationRequested)
                return;

            try
            {
                Card card = _deckManager.DrawCard();
                Hand.AddCard(card);
                await card.DrawFromDeck(handAnchor.position, _cts)
                    .AttachExternalCancellation(_cts.Token);
                Hand.UpdateHandLayout(handAnchor, spacing);
            }
            catch (OperationCanceledException)
            {
                return;
            }
        }
    }

    private void StartDealerTurn(GameEventBase e)
    {
        DealerTurnAsync(_cts).Forget();
    }
    private async UniTask DealerTurnAsync(CancellationTokenSource _cts)
    {
        try
        {
            await ShowCard(_cts);
            while (Hand.CalculateScore() < 17)
            {
                await DrawCardAsync(_cts);
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
        catch (OperationCanceledException)
        {
            Debug.LogWarning("Dealer turn canceled");
        }
    }
}
