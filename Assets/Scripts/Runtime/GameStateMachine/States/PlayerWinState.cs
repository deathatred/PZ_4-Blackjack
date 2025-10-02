using Cysharp.Threading.Tasks;
using System.Runtime.CompilerServices;
using UnityEngine;
using Zenject;

public class PlayerWinState : GameStateBase
{
    private bool _isWaiting = false;    
    [Inject]
    public PlayerWinState(GameStateMachine fsm) : base(fsm)
    {
    }

    public override void Enter()
    {
        EventBus.Publish(new PlayerWinEvent());

    }

    public override void Exit()
    {
     
    }

    public async override void Update()
    {
       if (!_isWaiting)
        {
            _isWaiting = true;
            await UniTask.WaitForSeconds(2);
            _fsm.ChangeState(GameState.ResetingTable);
        }
    }
}
