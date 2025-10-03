using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MainMenuViewUI : MonoBehaviour
{
    [SerializeField] private Button _playButton;
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
    }
    private void UnbindButtons()
    {
        _playButton.onClick.RemoveListener(PlayButtonPress);
    }
    private void PlayButtonPress()
    {
        _eventBus.Publish(new PlayPressedEvent());
    }
}
