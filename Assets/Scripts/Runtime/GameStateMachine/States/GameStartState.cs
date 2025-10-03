using UnityEngine;
using Zenject;

public class GameStartState : GameStateBase
{
    private bool _playPressed;
    [Inject]
    public GameStartState(GameStateMachine fsm) : base(fsm)
    {
    }

    public override void Enter()
    {
        EventBus.Subscribe<PlayPressedEvent>(PlayPressed);
    }

    public override void Exit()
    {
        EventBus.Unsubscribe<PlayPressedEvent>(PlayPressed);
    }

    public override void Update()
    {
        if (_playPressed)
        {
            _fsm.ChangeState(GameState.Betting);
        }
    }
    private void PlayPressed(GameEventBase e)
    {
        _playPressed = true;
    }
}
