using UnityEngine;
using static PlayerInputAction;

public class PlayerInputHandler : MonoBehaviour, IPlayerActions
{
    [SerializeField] PlayerController controller; //TODO : FIX THIS LATER BRUH
    public void OnTakeCard(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            controller.TakeCard();
        }
    }

    private void Awake()
    {
        InputManager.EnablePlayerInputActions();
        InputManager.SubscribeToPlayerInputActions(this);
    }
}
