using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class DealerTurnState : GameStateBase
{
    private bool _dealerTurnEnded = false;
    private bool _dealerLost = false;   
    private bool _isWaiting = false;
    [Inject]
    public DealerTurnState(GameStateMachine fsm) : base(fsm)
    {
    }

    public override void Enter()
    {
        EventBus.Publish(new DealerTurnStartedEvent());
        EventBus.Subscribe<DealerTurnEndedEvent>(EndDealerTurn);
        EventBus.Subscribe<PlayerWinEvent>(DealerLost);
    }

    public override void Exit()
    {

    }

    public override void Update()
    {
        if (_dealerLost)
        {
            _fsm.ChangeState(GameState.ResetingTable);
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
        await UniTask.WaitForSeconds(2f);
        _fsm.ChangeState(GameState.ResetingTable);
        _isWaiting = false;
    }
}
