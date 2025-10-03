using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class DealerTurnState : GameStateBase
{
    private readonly EventBus _eventBus;
    private bool _dealerTurnEnded = false;
    private bool _dealerLost = false;   
    private bool _isWaiting = false;
    [Inject]
    public DealerTurnState(GameStateMachine fsm, EventBus eventBus) : base(fsm)
    {
        _eventBus = eventBus;
    }

    public override void Enter()
    {
        _eventBus.Publish(new DealerTurnStartedEvent());
        _eventBus.Subscribe<DealerTurnEndedEvent>(EndDealerTurn);
        _eventBus.Subscribe<PlayerWinEvent>(DealerLost);
    }

    public override void Exit()
    {
        _eventBus.Unsubscribe<DealerTurnEndedEvent>(EndDealerTurn);
        _eventBus.Unsubscribe<PlayerWinEvent>(DealerLost);
    }

    public override void Update()
    {
        if (_dealerLost)
        {
            _fsm.ChangeState(GameState.PlayerWin);
        }
        if (_dealerTurnEnded)
        {
            if (!_isWaiting)
            {
                _isWaiting = true;
                StartNextDealAsync().Forget();
            }
            }
        }
    private void EndDealerTurn(DealerTurnEndedEvent e)
    {
        _dealerTurnEnded = true;
    }
    private void DealerLost(GameEventBase e)
    {
        _dealerLost = true;
    }
    private async UniTask StartNextDealAsync()
    {
        await UniTask.WaitForSeconds(1f);
        _fsm.ChangeState(GameState.DealerWin);
        _isWaiting = false;
    }
}
