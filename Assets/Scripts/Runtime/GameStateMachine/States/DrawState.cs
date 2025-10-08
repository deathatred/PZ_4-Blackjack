using Cysharp.Threading.Tasks;
using UnityEngine;

public class DrawState : GameStateBase
{
    private EventBus _eventBus;
    private bool _isWaiting;
    public DrawState(GameStateMachine fsm, EventBus eventBus) : base(fsm)
    {
        _eventBus = eventBus;
    }

    public override void Enter()
    {
        _eventBus.Publish(new DrawEvent());
        StartResetingAsync().Forget();

    }
    public override void Exit()
    {
        
    }

    public override void Update()
    {
  
    }

    private async UniTask StartResetingAsync()
    {
        await UniTask.WaitForSeconds(2f);
        _fsm.ChangeState(GameState.ResetingTable);
    }
}
