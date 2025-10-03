using UnityEngine;
using UnityEngine.UI;

public class MainMenuViewUI : MonoBehaviour
{
    [SerializeField] private Button _playButton;

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
        EventBus.Publish(new PlayPressedEvent());
    }
}
