using UnityEngine;

public abstract class GameStateBase
{
    protected GameStateMachine _fsm;

    public GameStateBase(GameStateMachine fsm)
    {
        _fsm = fsm;
    }
    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}
