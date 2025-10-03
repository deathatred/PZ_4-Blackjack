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
        Init(); // Zenject ������� ���� ����, �� �� ������� �����
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
