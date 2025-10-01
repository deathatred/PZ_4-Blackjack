using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class ResetingTableState : GameStateBase
{
    [Inject] private PlayerController _playerController;
    [Inject] private DeckManager _deckManager;
    private bool _isWaiting = false;
    public ResetingTableState(GameStateMachine fsm) : base(fsm)
    {
    }

    public override void Enter()
    {
        Debug.Log("ENTERED RESET TABLE");
        EventBus.Publish(new EnteredResetStateEvent());
    }

    public override void Exit()
    {

    }

    public override void Update()
    {
        if (!_isWaiting)
        {
            _isWaiting = true;
            StartNextDealAsync().Forget();
        }
    }
    private async UniTask StartNextDealAsync()
    {
        await UniTask.WaitForSeconds(2f);
        _fsm.ChangeState(GameState.Dealing);
        _isWaiting = false; 
    }
}
