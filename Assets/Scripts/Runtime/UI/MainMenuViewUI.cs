using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MainMenuViewUI : MonoBehaviour
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _historyButton;
    private EventBus _eventBus;

    [Inject]
    public void Construct(EventBus eventBus)
    {
        _eventBus =  eventBus;
    }
    private void Start()
    {
        BindButtons();
    }
    private void OnDestroy()
    {
        UnbindButtons();
    }
    private void BindButtons()
    {
        _playButton.onClick.AddListener(PlayButtonPress);
        _historyButton.onClick.AddListener(HistoryButtonPress);
    }
    private void UnbindButtons()
    {
        _playButton.onClick.RemoveListener(PlayButtonPress);
        _historyButton.onClick.RemoveListener(HistoryButtonPress);
    }
    private void PlayButtonPress()
    {
        _eventBus.Publish(new PlayPressedEvent());
    }
    private void HistoryButtonPress()
    {
        _eventBus.Publish(new HistoryPressedEvent());
    }
}
