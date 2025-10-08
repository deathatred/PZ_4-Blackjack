using Cysharp.Threading.Tasks;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using Zenject;

public class PlayerWinState : GameStateBase
{
    private readonly EventBus _eventBus;
    private bool _isWaiting = false;
    private Dealer _dealer;
    private CancellationTokenSource _cts = new CancellationTokenSource();
    [Inject]
    public PlayerWinState(GameStateMachine fsm, Dealer dealer, EventBus eventBus) : base(fsm)
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
        _cts.Cancel();
    }

    public override void Update()
    {
        HandleUpdateAsync().Forget();
    }
    private async UniTask HandleUpdateAsync()
    {
        if (!_isWaiting)
        {
            _isWaiting = true;
            try
            {
                await _dealer.ShowCard(_cts);
                await UniTask.WaitForSeconds(2).AttachExternalCancellation(_cts.Token);
                _fsm.ChangeState(GameState.ResetingTable);  
            }
            catch (OperationCanceledException)
            {
                Debug.LogWarning("Player win update Canceled");
            }
        }
    }

}
