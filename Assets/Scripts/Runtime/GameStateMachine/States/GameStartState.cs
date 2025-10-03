using UnityEngine;
using Zenject;

public class GameStartState : GameStateBase
{
    private readonly EventBus _eventBus;
    private bool _playPressed;
    [Inject]
    public GameStartState(GameStateMachine fsm, EventBus eventBus) : base(fsm)
    {
        _eventBus = eventBus;
    }

    public override void Enter()
    {
        _eventBus.Subscribe<PlayPressedEvent>(PlayPressed);
    }

    public override void Exit()
    {
        _eventBus.Unsubscribe<PlayPressedEvent>(PlayPressed);
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
