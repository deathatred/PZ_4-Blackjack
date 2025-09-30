using UnityEngine;
using Zenject;

public class DealerTurnState : GameStateBase
{
    private bool _dealerTurnEnded = false;
    [Inject]
    public DealerTurnState(GameStateMachine fsm) : base(fsm)
    {
    }

    public override void Enter()
    {
        EventBus.Publish(new DealerTurnStartedEvent());
        EventBus.Subscribe<DealerTurnEndedEvent>(EndDealerTurn);
    }

    public override void Exit()
    {

    }

    public override void Update()
    {
        if (_dealerTurnEnded)
        {
            _fsm.ChangeState(GameState.ComparingHands);
        }
    }
    private void EndDealerTurn(DealerTurnEndedEvent e)
    {
        _dealerTurnEnded = true;
    }
}
