using UnityEngine;
using Zenject;

public class DealingState : GameStateBase
{
    private bool _dealingFinished = false;
    private bool _playerBlackjack = false;
    private DeckManager _deckManager;

    [Inject]
    public DealingState(DeckManager deckManager,GameStateMachine fsm) : base(fsm)
    {
        _deckManager = deckManager;
    }
    public override void Enter()
    {
        Debug.Log(_deckManager.GetDeckCount());
        EventBus.Publish(new DealingStartedEvent());
        EventBus.Subscribe<DealingFinishedEvent>(DealingFinished);
        EventBus.Subscribe<PlayerBlackjackEvent>(PlayerBlackjack);
    }
    public override void Exit()
    {
        EventBus.Unsubscribe<DealingFinishedEvent>(DealingFinished);
        EventBus.Unsubscribe<PlayerBlackjackEvent>(PlayerBlackjack);
    }
    public override void Update()
    {
        if (_playerBlackjack)
        {
            _fsm.ChangeState(GameState.DealerTurn);
        }
        if (_dealingFinished)
        {
            _fsm.ChangeState(GameState.PlayerTurn);
        }
       
    }
    private void PlayerBlackjack(GameEventBase e)
    {
        _playerBlackjack = true;
    }
    private void DealingFinished(GameEventBase e)
    {
        _dealingFinished = true;
    }
}
