using UnityEngine;
using Zenject;

public class DealingState : GameStateBase
{
    private bool _dealingFinished = false;
    private bool _playerBlackjack = false;
    private DeckManager _deckManager;
    private readonly EventBus _eventBus;

    [Inject]
    public DealingState(DeckManager deckManager,GameStateMachine fsm, EventBus eventBus) : base(fsm)
    {
        _deckManager = deckManager;
        _eventBus = eventBus;
    }
    public override void Enter()
    {
        Debug.Log(_deckManager.GetDeckCount());
        _eventBus.Publish(new DealingStartedEvent());
        _eventBus.Subscribe<DealingFinishedEvent>(DealingFinished);
        _eventBus.Subscribe<PlayerBlackjackEvent>(PlayerBlackjack);
    }
    public override void Exit()
    {
        _eventBus.Unsubscribe<DealingFinishedEvent>(DealingFinished);
        _eventBus.Unsubscribe<PlayerBlackjackEvent>(PlayerBlackjack);
    }
    public override void Update()
    {
        if (_playerBlackjack)
        {
            _fsm.ChangeState(GameState.PlayerWin);
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
