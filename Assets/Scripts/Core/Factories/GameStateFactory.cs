using UnityEngine;
using Zenject;
using Zenject.Asteroids;

public class GameStateFactory 
{
    private readonly DiContainer _container;
    public GameStateFactory(DiContainer container)
    {
        _container = container;
    }
    public T CreateState<T>() where T : GameStateBase
    {
        return _container.Instantiate<T>();
    }
    public GameStateBase CreateGameState(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.GameStart:
                return CreateState<GameStartState>();
            case GameState.Dealing:
                return CreateState<DealingState>();
            case GameState.PlayerTurn:
                return CreateState<PlayerTurnState>();
            case GameState.DealerTurn:
                return CreateState<DealerTurnState>();
            case GameState.ComparingHands:
                return CreateState<ComparingHandsState>();
            case GameState.PlayerWin:
                return CreateState<PlayerWinState>();
            case GameState.DealerWin:
                return CreateState<DealerWinState>();
            case GameState.GameOver:
                return null;
        }
        Debug.LogError("State does not exist");
        return null;
    }
}