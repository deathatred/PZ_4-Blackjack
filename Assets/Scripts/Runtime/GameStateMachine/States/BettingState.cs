using UnityEngine;

public class BettingState : GameStateBase
{
    private readonly EventBus _eventBus;
    private bool _bettingEnded;
    public BettingState(GameStateMachine fsm, EventBus eventBus) : base(fsm)
    {
        _eventBus = eventBus;
    }

    public override void Enter()
    {
        _eventBus.Publish(new BettingStartedEvent());
        _eventBus.Subscribe<BettingEndedEvent>(EndBetting);
    }

    public override void Exit()
    {
        _eventBus.Unsubscribe<BettingEndedEvent>(EndBetting);
    }

    public override void Update()
    {
        if (_bettingEnded)
        {
            Debug.Log("Bet placed, changing state to Dealing");
            _fsm.ChangeState(GameState.Dealing);  
        }
    }
    private void EndBetting(GameEventBase e)
    {
        _bettingEnded = true;
    }
}
