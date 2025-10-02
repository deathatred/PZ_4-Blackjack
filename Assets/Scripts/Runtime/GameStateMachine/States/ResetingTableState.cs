using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class ResetingTableState : GameStateBase
{
    private PlayerController _playerController;
    private DeckManager _deckManager;
    private MoneyManager _moneyManager;
    private bool _isWaiting = false;
    public ResetingTableState(PlayerController player,
        DeckManager deckManager,
        MoneyManager moneyManager, GameStateMachine fsm) : base(fsm)
    {
        _playerController = player;
        _deckManager = deckManager;
        _moneyManager = moneyManager;
    }

    public override void Enter()
    {
        EventBus.Publish(new EnteredResetStateEvent());
    }

    public override void Exit()
    {

    }

    public override void Update()
    {
        if (_moneyManager.MoneyAmount == 0)
        {
            _fsm.ChangeState(GameState.GameOver);
            return;
        }
        if (!_isWaiting)
        {
            _isWaiting = true;
            StartNextDealAsync().Forget();
        }
    }
    private async UniTask StartNextDealAsync()
    {
        await UniTask.WaitForSeconds(2f);
        _fsm.ChangeState(GameState.Betting);
        _isWaiting = false; 
    }
}
