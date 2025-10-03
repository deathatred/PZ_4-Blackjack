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
        Debug.Log("ented Start");
        _eventBus.Subscribe<PlayPressedEvent>(PlayPressed);
    }

    public override void Exit()
    {
        _eventBus.Unsubscribe<PlayPressedEvent>(PlayPressed);
    }

    public override void Update()
    {
        //Debug.Log(_playPressed);
        //if (_playPressed)
        //{
            _fsm.ChangeState(GameState.Betting);
        //}
    }
    private void PlayPressed(GameEventBase e)
    {
        _playPressed = true;
    }
}
