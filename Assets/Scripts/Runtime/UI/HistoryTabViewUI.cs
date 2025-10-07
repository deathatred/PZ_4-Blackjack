using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Zenject;

public class HistoryTabViewUI : MonoBehaviour
{
    [SerializeField] private ScrollView _gamesHistoryScrollView;
    [SerializeField] private UnityEngine.UI.Button _backButton;
    [Inject] private EventBus _eventBus;
    private void Awake()
    {
        BindButtons();
    }
    private void OnDisable()
    {
        UnbindButtons();
    }
    private void BindButtons()
    {
        _backButton.onClick.AddListener(BackButtonPressed);
    }
    private void UnbindButtons()
    {
        _backButton.onClick.RemoveListener(BackButtonPressed);
    }
    private void BackButtonPressed()
    {
        _eventBus.Publish(new BackPressedEvent());
    }
}
