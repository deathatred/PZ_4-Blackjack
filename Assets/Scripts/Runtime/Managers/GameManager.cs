using System.Runtime.CompilerServices;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
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
        _gameStateMachine = gameStateMachine;
    }
    public void Update()
    {
        Debug.Log(_gameStateMachine.CurrentGameState);
        _gameStateMachine.StateUpdate();
    }
}
