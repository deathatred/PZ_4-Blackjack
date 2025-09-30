using UnityEngine;
using Zenject;

public class DealingState : GameStateBase
{
    private bool _dealingFinished = false;
    private bool _playerBlackjack = false;

    [Inject]
    public DealingState(GameStateMachine fsm) : base(fsm)
    {
    }
    public override void Enter()
    {
        EventBus.Publish(new DealingStartedEvent());
        EventBus.Subscribe<DealingFinishedEvent>(DealingFinished);
        EventBus.Subscribe<PlayerBlackjackEvent>(PlayerBlackjack);
    }
    public override void Exit()
    {

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
