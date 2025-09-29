using Cysharp.Threading.Tasks;
using UnityEngine;
using static PlayerInputAction;

public class PlayerInputHandler : MonoBehaviour, IPlayerActions
{
    [SerializeField] PlayerController controller;
    public void OnTakeCard(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            controller.TakeCard().Forget();
        }
    }

    private void Awake()
    {
        InputManager.EnablePlayerInputActions();
        InputManager.SubscribeToPlayerInputActions(this);
    }
}
