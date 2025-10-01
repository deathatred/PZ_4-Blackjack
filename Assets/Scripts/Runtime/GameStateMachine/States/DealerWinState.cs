using UnityEngine;
using Zenject;

public class DealerWinState : GameStateBase
{
    private Dealer _dealer;
    private bool _cardsShown = false;
    [Inject]
    public DealerWinState(Dealer dealer, GameStateMachine fsm) : base(fsm)
    {
        _dealer = dealer;
    }

    public async override void Enter()
    {
        await _dealer.ShowCard();
        _cardsShown = true;
    }

    public override void Exit()
    {

    }

    public override void Update()
    {
        if (_cardsShown)
        {
            _fsm.ChangeState(GameState.ResetingTable);
        }
    }
}
