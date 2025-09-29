using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField] private Image _frontImage;
    [SerializeField] private Image _backImage;

    private CardData _data;

    public void Setup(CardData data)
    {
        _data = data;
        _frontImage.sprite = data.FrontImage;
        _backImage.sprite = data.BackImage;
    }
}
