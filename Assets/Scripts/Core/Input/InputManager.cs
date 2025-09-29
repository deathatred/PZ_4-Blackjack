using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerInputAction;

public static class InputManager 
{
    private static PlayerInputAction _playerInput = new PlayerInputAction();

    public static void EnablePlayerInputActions()
    {
        _playerInput.Player.Enable();
    }
    public static void DisablePlayerInputActions()
    {
        _playerInput.Player.Disable();
    }
    public static void SubscribeToPlayerInputActions(IPlayerActions input)
    {
        _playerInput.Player.SetCallbacks(input);
    }
    public static void UnsubscribeToPlayerInputActions(IPlayerActions input)
    {
        _playerInput.Player.RemoveCallbacks(input);
    }
}
