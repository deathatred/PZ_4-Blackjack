using UnityEngine;
using UnityEngine.Experimental.AI;
using Zenject;

public class PlayerTurnState : GameStateBase
{
    private readonly EventBus _eventBus;
    private bool _playerTurnEnded = false;
    private bool _dealerWin = false;
    [Inject]
    public PlayerTurnState(GameStateMachine fsm, EventBus eventBus) : base(fsm)
    {
        _eventBus = eventBus;
    }

    public override void Enter()
    {
        //Activate UI buttons
        _eventBus.Publish(new PlayerTurnStartedEvent());
        _eventBus.Subscribe<PushButtonPressedEvent>(PlayerTurnEnded);
        _eventBus.Subscribe<PlayerOverflowEvent>(DealerWin);
    }

    public override void Exit()
    {

        _eventBus.Publish(new PlayerTurnEndedEvent());
        _eventBus.Unsubscribe<PushButtonPressedEvent>(PlayerTurnEnded);
        _eventBus.Unsubscribe<PlayerOverflowEvent>(DealerWin);
    }

    public override void Update()
    {
        if (_dealerWin)
        {
            _fsm.ChangeState(GameState.DealerWin);
        }
        if (_playerTurnEnded)
        {
            _fsm.ChangeState(GameState.DealerTurn);
        }
    }
    private void PlayerTurnEnded(PushButtonPressedEvent e)
    {
       _playerTurnEnded = true; 
    }
    private void DealerWin(PlayerOverflowEvent e)
    {
        _dealerWin = true;
    }
}
