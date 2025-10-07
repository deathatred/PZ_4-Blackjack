using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoundHistoryTabSingle : MonoBehaviour
{
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private TextMeshProUGUI _roundNumberText;
    [SerializeField] private TextMeshProUGUI _gameConclusionText;
    [SerializeField] private TextMeshProUGUI _moneyChangeText;
    [SerializeField] private Color _winColor;
    [SerializeField] private Color _loseColor;

    public void Init(GameResult result)
    {
        if (result.Result == GameOutcome.Win.ToString())
        {
            _backgroundImage.color = _winColor;
        }
        else
        {
            _backgroundImage.color = _loseColor;
        }
        _roundNumberText.text = result.RoundNumber.ToString();
        _gameConclusionText.text = result.Result.ToString();
        _moneyChangeText.text = result.MoneyChange.ToString();
    }
}
