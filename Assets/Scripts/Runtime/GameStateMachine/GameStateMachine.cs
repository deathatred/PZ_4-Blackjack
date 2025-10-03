using UnityEngine;
using Zenject;

public class GameStateMachine
{
    public GameStateBase CurrentGameState { get; private set; }
    
    private GameStateFactory _gameStateFactory;

    [Inject]
    public GameStateMachine(GameStateFactory stateFactory)
    {
        _gameStateFactory = stateFactory;
    }
    [Inject]
    public void Initialize()
    {
        Init(); // Zenject викликає після того, як всі інжекти готові
    }
    public void Init()
    {

        ChangeState(GameState.GameStart);
    }
    public void ChangeState(GameState newState)
    {
        CurrentGameState?.Exit();
        CurrentGameState = _gameStateFactory.CreateGameState(newState);
        CurrentGameState.Enter();
    }
    public void StateUpdate()
    {
        CurrentGameState.Update();
    }

}
