using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class DealerWinState : GameStateBase
{
    private Dealer _dealer;
    private readonly EventBus _eventBus;
    private bool _flippedCard;

    [Inject]
    public DealerWinState(Dealer dealer, GameStateMachine fsm, EventBus eventBus) : base(fsm)
    {
        _dealer = dealer;
        _eventBus = eventBus;
    }

    public override void Enter()
    {
        _eventBus.Publish(new DealerWinEvent());
        WaitToFlipCard().Forget();
    }

    public override void Exit()
    {

    }

    public override void Update()
    {
        if (_flippedCard)
        {
            _fsm.ChangeState(GameState.ResetingTable);
        }
    }
    public async UniTask WaitToFlipCard()
    {
        await _dealer.ShowCard();
        await UniTask.WaitForSeconds(2f);
        _flippedCard = true;
    }
}
