using UnityEngine;
using UnityEngine.SceneManagement;

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
        SceneManager.sceneLoaded += SceneReloaded;
    }

    private void SceneReloaded(Scene arg0, LoadSceneMode arg1)
    {
        _fsm.ChangeState(GameState.GameStart);
    }

    public override void Exit()
    {
   
    }

    public override void Update()
    {
       
    }
}
