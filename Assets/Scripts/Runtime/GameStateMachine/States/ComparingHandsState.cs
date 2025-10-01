using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class ComparingHandsState : GameStateBase
{
    private PlayerController _player;
    private Dealer _dealer;

    [Inject]
    public ComparingHandsState(GameStateMachine fsm, PlayerController player, Dealer dealer)
        : base(fsm)
    {
        _player = player;
        _dealer = dealer;
    }

    public async override void Enter()
    {
        await CompareScores();
    }

    public override void Exit()
    {

    }

    public override void Update()
    {

    }
    private async UniTask CompareScores()
    {
        int playerRes = _player.Hand.CalculateScore();
        int dealerRes = _dealer.Hand.CalculateScore();
        if (playerRes > dealerRes)
        {
            Debug.Log("player wins");
            EventBus.Publish(new PlayerWinEvent());
            await UniTask.WaitForSeconds(2.5f);
            _fsm.ChangeState(GameState.ResetingTable);
        }
        else if (dealerRes > playerRes)
        {
            Debug.Log("player lost(");
            EventBus.Publish(new DealerWinEvent());
            await UniTask.WaitForSeconds(2.5f);
            _fsm.ChangeState(GameState.ResetingTable);
        }

    }
}
