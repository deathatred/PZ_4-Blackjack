using UnityEngine;

public class GameOverState : GameStateBase
{
    private readonly EventBus _eventBus;

    public GameOverState(GameStateMachine fsm, EventBus eventBus) : base(fsm)
    {
        _eventBus = eventBus;
    }

    public override void Enter()
    {
        _eventBus.Publish(new GameOverEvent());
    }

    public override void Exit()
    {
   
    }

    public override void Update()
    {
       
    }
}
