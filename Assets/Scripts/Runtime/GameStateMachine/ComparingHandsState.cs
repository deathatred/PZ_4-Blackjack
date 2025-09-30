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

    public override void Enter()
    {
        CompareScores();
    }

    public override void Exit()
    {
       
    }

    public override void Update()
    {
       
    }
    private void CompareScores()
    {
        int playerRes = _player.Hand.CalculateScore();
        int dealerRes = _dealer.Hand.CalculateScore();
        if (playerRes>dealerRes)
        {
            Debug.Log("player wins");
            EventBus.Publish(new PlayerWinEvent());
        }
        else if (dealerRes>playerRes)
        {
            Debug.Log("player lost(");
            EventBus.Publish(new DealerWinEvent());
        }

    }
}
