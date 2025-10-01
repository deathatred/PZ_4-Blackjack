using System.Runtime.CompilerServices;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private DeckManager _deckManager;
    private GameStateMachine _gameStateMachine;
    [Inject]
    public void Construct(DeckManager deckManager, GameStateMachine gameStateMachine)
    {
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
    private void OnDestroy()
    {
       _deckManager.Dispose();
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
