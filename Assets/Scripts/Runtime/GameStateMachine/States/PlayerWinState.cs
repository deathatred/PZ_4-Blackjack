using Cysharp.Threading.Tasks;
using System.Runtime.CompilerServices;
using UnityEngine;
using Zenject;

public class PlayerWinState : GameStateBase
{
    private readonly EventBus _eventBus;
    private bool _isWaiting = false;  
    private Dealer _dealer;
    [Inject]
    public PlayerWinState(GameStateMachine fsm,Dealer dealer, EventBus eventBus) : base(fsm)
    {
        _eventBus = eventBus;
        _dealer = dealer;
    }

    public override void Enter()
    {
        _eventBus.Publish(new PlayerWinEvent());

    }

    public override void Exit()
    {
     
    }

    public async override void Update()
    {
       if (!_isWaiting)
        {
            _isWaiting = true;
            await _dealer.ShowCard();
            await UniTask.WaitForSeconds(2);
            _fsm.ChangeState(GameState.ResetingTable);
        }
    }
}
