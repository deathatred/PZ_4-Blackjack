using UnityEngine;
using Zenject;

public class GameStartState : GameStateBase
{
    [Inject]
    public GameStartState(GameStateMachine fsm) : base(fsm)
    {
    }

    public override void Enter()
    {
    }

    public override void Exit()
    {

    }

    public override void Update()
    {
        _fsm.ChangeState(GameState.Betting);
    }
}
