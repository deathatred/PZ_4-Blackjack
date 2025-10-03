using UnityEngine;
using UnityEngine.Experimental.AI;
using Zenject;

public class PlayerTurnState : GameStateBase
{
    private bool _playerTurnEnded = false;
    private bool _dealerWin = false;
    [Inject]
    public PlayerTurnState(GameStateMachine fsm) : base(fsm)
    {
    }

    public override void Enter()
    {
        //Activate UI buttons
        EventBus.Publish(new PlayerTurnStartedEvent());
        EventBus.Subscribe<PushButtonPressedEvent>(PlayerTurnEnded);
        EventBus.Subscribe<DealerWinEvent>(DealerWin);
    }

    public override void Exit()
    {
        Debug.Log("Player turn ended bruh");
        EventBus.Publish(new PlayerTurnEndedEvent());
        EventBus.Unsubscribe<PushButtonPressedEvent>(PlayerTurnEnded);
        EventBus.Unsubscribe<DealerWinEvent>(DealerWin);
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
    private void DealerWin(DealerWinEvent e)
    {
        _dealerWin = true;
    }
}
