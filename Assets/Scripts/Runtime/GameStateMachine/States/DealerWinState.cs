using UnityEngine;
using Zenject;

public class DealerWinState : GameStateBase
{
    private Dealer _dealer;
    [Inject]
    public DealerWinState(Dealer dealer, GameStateMachine fsm) : base(fsm)
    {
        _dealer = dealer;
    }

    public override void Enter()
    {
        EventBus.Publish(new DealerWinEvent());
    }

    public override void Exit()
    {

    }

    public override void Update()
    {
        _fsm.ChangeState(GameState.ResetingTable);
    }
}
