using UnityEngine;
using Zenject;

public class PlayerWinState : GameStateBase
{
    [Inject]
    public PlayerWinState(GameStateMachine fsm) : base(fsm)
    {
    }

    public override void Enter()
    {
        throw new System.NotImplementedException();
    }

    public override void Exit()
    {
        throw new System.NotImplementedException();
    }

    public override void Update()
    {
        throw new System.NotImplementedException();
    }
}
