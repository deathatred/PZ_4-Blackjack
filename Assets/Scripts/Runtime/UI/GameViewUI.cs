using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameViewUI : MonoBehaviour
{
    [SerializeField] private Image _endOfRoundBackground;
    [SerializeField] private Button _takeButton;
    [SerializeField] private Button _pushButton;
    [SerializeField] private TextMeshProUGUI _dealerScoreText;
    [SerializeField] private TextMeshProUGUI _playerScoreText;
    [SerializeField] private TextMeshProUGUI _playerMoneyText;
    [SerializeField] private TextMeshProUGUI _endOfRoundText;
    [SerializeField] private RectTransform _endOfTurnObj;
    [SerializeField] private RectTransform _bettingObj;
    [SerializeField] private TextMeshProUGUI _betMoneyText;
    [SerializeField] private Button _plusButton;
    [SerializeField] private Button _minusButton;
    [SerializeField] private Button _placeBetButton;

    [Inject] private MoneyManager _moneyManager;

    private int _betModifier = 100;
    private int _betMoney = 0;

    private Action<PlayerTurnStartedEvent> _onPlayerTurnStart;
    private Action<PlayerTurnEndedEvent> _onPlayerTurnEnd;
    private Action<DealingCardsToPlayerEndedEvent> _onDealingEnded;
    private Action<DealingCardsToPlayerStartedEvent> _onDealingStarted;
    private Action<MoneyChangedEvent> _onMoneyChanged;
    private Action<PlayerWinEvent> _onPlayerWin;
    private Action<DealerWinEvent> _onPlayerLose;
    private Action<DrawEvent> _onPlayerDraw;
    private Action<BettingStartedEvent> _onResetEndOfTurn;
    private Action<BettingStartedEvent> _onShowBetMenu;
    private Action<BettingEndedEvent> _onHideBetMenu;
    private Action<PlayerDrawnCardEvent> _onPlayerDrawnCard;
    private Action<DealerDrawnCardEvent> _onDealerDrawnCard;
    
    private EventBus _eventBus;

    [Inject]
    public void Construct(EventBus eventBus)
    {
        _eventBus =  eventBus;
    }

    private void Awake()
    {
        _betMoney = _betModifier;
        BindButtons();
        SetButtonsOff(new PlayerTurnEndedEvent());
    }

    private void OnEnable() => SubscribeToEvents();
    private void OnDisable()
    {
        UnsubscribeFromEvents();
        UnbindButtons();
    }
    private void OnDestroy()
    {
        UnsubscribeFromEvents();
        UnbindButtons();
    }

    private void BindButtons()
    {
        _takeButton.onClick.AddListener(TakePressed);
        _pushButton.onClick.AddListener(PushPressed);
        _plusButton.onClick.AddListener(AddMoneyToBet);
        _minusButton.onClick.AddListener(RemoveMoneyFromBet);
        _placeBetButton.onClick.AddListener(PlaceBet);
    }

    private void UnbindButtons()
    {
        _takeButton.onClick.RemoveListener(TakePressed);
        _pushButton.onClick.RemoveListener(PushPressed);
        _plusButton.onClick.RemoveListener(AddMoneyToBet);
        _minusButton.onClick.RemoveListener(RemoveMoneyFromBet);
        _placeBetButton.onClick.RemoveListener(PlaceBet);
    }

    private void TakePressed() => _eventBus.Publish(new TakeButtonPressedEvent());
    private void PushPressed() => _eventBus.Publish(new PushButtonPressedEvent());

    private void SubscribeToEvents()
    {
        _onPlayerTurnStart = e => SetButtonsOn(e);
        _onPlayerTurnEnd = e => SetButtonsOff(e);
        _onDealingEnded = e => SetButtonsOn(e);
        _onDealingStarted = e => SetButtonsOff(e);

        _onMoneyChanged = e => ChangeMoney(e);
        _onPlayerWin = e => PlayerWinRoundMsg(e);
        _onPlayerLose = e => PlayerLostRoundMsg(e);
        _onResetEndOfTurn = e => ResetComponentsAtTheEndOfTurn(e);
        _onShowBetMenu = e => ShowBetMenu(e);
        _onHideBetMenu = e => HideBetMenu(e);
        _onPlayerDrawnCard = e => ChangePlayerScore(e);
        _onDealerDrawnCard = e => ChangeDealerScore(e);
        _onPlayerDraw = e => _onPlayerDraw(e);

        _eventBus.Subscribe(_onPlayerTurnStart);
        _eventBus.Subscribe(_onPlayerTurnEnd);
        _eventBus.Subscribe(_onDealingEnded);
        _eventBus.Subscribe(_onDealingStarted);
        _eventBus.Subscribe(_onMoneyChanged);
        _eventBus.Subscribe(_onPlayerWin);
        _eventBus.Subscribe(_onPlayerLose);
        _eventBus.Subscribe(_onResetEndOfTurn);
        _eventBus.Subscribe(_onShowBetMenu);
        _eventBus.Subscribe(_onHideBetMenu);
        _eventBus.Subscribe(_onPlayerDrawnCard);
        _eventBus.Subscribe(_onDealerDrawnCard);
        _eventBus.Subscribe(_onPlayerDraw);
    }

    private void UnsubscribeFromEvents()
    {
        _eventBus.Unsubscribe(_onPlayerTurnStart);
        _eventBus.Unsubscribe(_onPlayerTurnEnd);
        _eventBus.Unsubscribe(_onDealingEnded);
        _eventBus.Unsubscribe(_onDealingStarted);
        _eventBus.Unsubscribe(_onMoneyChanged);
        _eventBus.Unsubscribe(_onPlayerWin);
        _eventBus.Unsubscribe(_onPlayerLose);
        _eventBus.Unsubscribe(_onResetEndOfTurn);
        _eventBus.Unsubscribe(_onShowBetMenu);
        _eventBus.Unsubscribe(_onHideBetMenu);
        _eventBus.Unsubscribe(_onPlayerDrawnCard);
        _eventBus.Unsubscribe(_onDealerDrawnCard);
        _eventBus.Unsubscribe(_onPlayerDraw);
    }

    private void SetButtonsOn(GameEventBase e)
    {
        _takeButton.gameObject.SetActive(true);
        _pushButton.gameObject.SetActive(true);
    }

    private void SetButtonsOff(GameEventBase e)
    {
        _takeButton.gameObject.SetActive(false);
        _pushButton.gameObject.SetActive(false);
    }

    private void ChangeMoney(MoneyChangedEvent e)
    {
        _playerMoneyText.text = $"{e.MoneyAmount}$";
    }

    private void PlayerLostRoundMsg(GameEventBase e)
    {
        _endOfTurnObj.gameObject.SetActive(true);
        _endOfRoundText.text = "you lost";
        _endOfRoundBackground.color = Color.red;
    }

    private void PlayerWinRoundMsg(GameEventBase e)
    {
        _endOfTurnObj.gameObject.SetActive(true);
        _endOfRoundText.text = "you win";
        _endOfRoundBackground.color = Color.green;
    }
    private void PlayerDrawRoundMsg(GameEventBase e)
    {
        _endOfTurnObj.gameObject.SetActive(true);
        _endOfRoundText.text = "draw";
        _endOfRoundBackground.color = Color.gray;
    }

    private void ResetComponentsAtTheEndOfTurn(GameEventBase e)
    {
        _endOfTurnObj.gameObject.SetActive(false);
        _betMoney = _betModifier;
        _betMoneyText.text = "100$";
        _playerScoreText.text = "0";
        _dealerScoreText.text= "0";
    }

    private void AddMoneyToBet()
    {
        if ((_betMoney + _betModifier) > _moneyManager.MoneyAmount) return;

        _betMoney += _betModifier;
        _betMoneyText.text = $"{_betMoney}$";
    }

    private void RemoveMoneyFromBet()
    {
        if ((_betMoney - _betModifier) <= 0) return;

        _betMoney -= _betModifier;
        _betMoneyText.text = $"{_betMoney}$";
    }

    private void PlaceBet()
    {
        _moneyManager.PlaceBet(_betMoney);
        _playerMoneyText.text = $"{_moneyManager.MoneyAmount}$";
    }

    private void ShowBetMenu(GameEventBase e) => _bettingObj.gameObject.SetActive(true);
    private void HideBetMenu(GameEventBase e) => _bettingObj.gameObject.SetActive(false);

    private void ChangePlayerScore(PlayerDrawnCardEvent e)
    {
        _playerScoreText.text = e.Score.ToString();
    }
    private void ChangeDealerScore(DealerDrawnCardEvent e)
    {
        _dealerScoreText.text = e.Score.ToString();
    }
}
