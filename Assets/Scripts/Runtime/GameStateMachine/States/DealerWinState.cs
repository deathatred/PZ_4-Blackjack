using UnityEngine;
using Zenject;

public class DealerWinState : GameStateBase
{
    private Dealer _dealer;
    private readonly EventBus _eventBus;

    [Inject]
    public DealerWinState(Dealer dealer, GameStateMachine fsm, EventBus eventBus) : base(fsm)
    {
        _dealer = dealer;
        _eventBus = eventBus;
    }

    public override void Enter()
    {
        _eventBus.Publish(new DealerWinEvent());
    }

    public override void Exit()
    {

    }

    public override void Update()
    {
        _fsm.ChangeState(GameState.ResetingTable);
    }
}
