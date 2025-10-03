using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameEndViewUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _gameEndText;
    [SerializeField] private Button _tryAgainButton;

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
        _tryAgainButton.onClick.AddListener(TryAgainButtonPress);
    }
    private void UnbindButtons()
    {
        _tryAgainButton.onClick.RemoveListener(TryAgainButtonPress);
    }
    private void  TryAgainButtonPress()
    {
        EventBus.Publish(new TryAgainPressedEvent());
    }
}
