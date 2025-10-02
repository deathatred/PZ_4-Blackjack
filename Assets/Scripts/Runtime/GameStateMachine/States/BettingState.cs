using UnityEngine;

public class BettingState : GameStateBase
{
    private bool _bettingEnded;
    public BettingState(GameStateMachine fsm) : base(fsm)
    {
       
    }

    public override void Enter()
    {
        EventBus.Publish(new BettingStartedEvent());
        EventBus.Subscribe<BettingEndedEvent>(EndBetting);
    }

    public override void Exit()
    {
        EventBus.Unsubscribe<BettingEndedEvent>(EndBetting);
    }

    public override void Update()
    { 
        if (_bettingEnded)
        {
            _fsm.ChangeState(GameState.Dealing);  
        }
    }
    private void EndBetting(GameEventBase e)
    {
        _bettingEnded = true;
    }
}
