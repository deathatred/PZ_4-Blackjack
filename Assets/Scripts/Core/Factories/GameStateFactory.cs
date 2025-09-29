using UnityEngine;

public class GameStateFactory 
{
    public GameStateBase CreateGameState(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.GameStart:
                return new GameStartState();
            case GameState.Dealing:
                return new DealingState();
            case GameState.PlayerTurn:
                return new PlayerTurnState();
            case GameState.DealerTurn:
                return new DealerTurnState();
            case GameState.ComparingHands:
                return new ComparingHandsState();
            case GameState.PlayerWin:
                return new PlayerWinState();
            case GameState.DealerWin:
                return new DealerWinState();
            case GameState.GameOver:
                return null;
        }
        Debug.LogError("State does not exist");
        return null;
    }
}