using UnityEngine;

public class GameOverState : GameStateBase
{
    public GameOverState(GameStateMachine fsm) : base(fsm)
    {
    }

    public override void Enter()
    {
        EventBus.Publish(new GameOverEvent());
    }

    public override void Exit()
    {
   
    }

    public override void Update()
    {
       
    }
}
