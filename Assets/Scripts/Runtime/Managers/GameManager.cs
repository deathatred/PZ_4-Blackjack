using System.Runtime.CompilerServices;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    private GameStateMachine _gameStateMachine;

    [Inject]
    public void Construct(DeckManager deckManager, 
        GameStateMachine gameStateMachine)
    {

        _gameStateMachine = gameStateMachine;
    }
    public void Update()
    {
        _gameStateMachine.StateUpdate();
    }
}
