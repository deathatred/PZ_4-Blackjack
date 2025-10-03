using System.Runtime.CompilerServices;
using UnityEngine;
using Zenject;

public class GameManager
{
    private DeckManager _deckManager;
    private MoneyManager _moneyManager;
    private GameStateMachine _gameStateMachine;
    private ViewManager _viewManager;
    [Inject]
    public void Construct(DeckManager deckManager, 
        ViewManager viewManager,
        MoneyManager moneyManager,
        GameStateMachine gameStateMachine)
    {
        _moneyManager = moneyManager;
        _viewManager = viewManager;
        _deckManager = deckManager;
        _deckManager.Init();
        _gameStateMachine = gameStateMachine;
        _gameStateMachine.Init();
    }
    public void Update()
    {
        Debug.Log(_gameStateMachine.CurrentGameState);
        _gameStateMachine.StateUpdate();
    }
}
