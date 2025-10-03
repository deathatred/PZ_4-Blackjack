using System.Runtime.CompilerServices;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
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
    public void Awake()
    {
        InitSingleton();
    }
    public void Update()
    {
        print(_gameStateMachine.CurrentGameState);
        _gameStateMachine.StateUpdate();
    }
    private void InitSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
}
