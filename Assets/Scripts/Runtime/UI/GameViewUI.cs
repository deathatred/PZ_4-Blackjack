using UnityEngine;
using UnityEngine.UI;

public class GameViewUI : MonoBehaviour
{
    [SerializeField] private Button _takeButton;
    [SerializeField] private Button _pushButton;

    private void OnEnable()
    {
        SubscribeToEvents();
    }
    private void Awake()
    {
        BindButtons();
        SetButtonsOff(new PlayerTurnEndedEvent());
    }
    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }
    private void BindButtons()
    {
        _takeButton.onClick.AddListener(TakePressed);
        _pushButton.onClick.AddListener(PushPressed);
    }
    private void TakePressed()
    {
        EventBus.Publish(new TakeButtonPressedEvent());
    }
    private void PushPressed()
    {
        EventBus.Publish(new PushButtonPressedEvent());
    }
    private void SubscribeToEvents()
    {
        EventBus.Subscribe<PlayerTurnStartedEvent>(SetButtonsOn);
        EventBus.Subscribe<PlayerTurnEndedEvent>(SetButtonsOff);
    }
    private void UnsubscribeFromEvents()
    {
        EventBus.Unsubscribe<PlayerTurnStartedEvent>(SetButtonsOn);
        EventBus.Unsubscribe<PlayerTurnEndedEvent>(SetButtonsOff);
    }
    private void SetButtonsOn(PlayerTurnStartedEvent e)
    {
        _takeButton.gameObject.SetActive(true);
        _pushButton.gameObject.SetActive(true);
    }
    private void SetButtonsOff(PlayerTurnEndedEvent e)
    {
        _takeButton.gameObject.SetActive(false);
        _pushButton.gameObject.SetActive(false);
    }
}
