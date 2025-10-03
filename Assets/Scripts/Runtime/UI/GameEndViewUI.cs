using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameEndViewUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _gameEndText;
    [SerializeField] private Button _tryAgainButton;
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
        _tryAgainButton.onClick.AddListener(TryAgainButtonPress);
    }
    private void UnbindButtons()
    {
        _tryAgainButton.onClick.RemoveListener(TryAgainButtonPress);
    }
    private void  TryAgainButtonPress()
    {
        _eventBus.Publish(new TryAgainPressedEvent());
    }
}
