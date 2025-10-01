using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Card : MonoBehaviour
{
    [Inject] private DeckManager _deckManager;
    [SerializeField] private SpriteRenderer _frontRenderer;
    [SerializeField] private SpriteRenderer _backRenderer;

    public bool IsFlipped { get; private set; } = false;
    private Quaternion _faceDownRotation = Quaternion.Euler(-90f, 0f, -180f);
    private Quaternion _faceUpRotation = Quaternion.Euler(90f, 180f, -180f);
    private CardDataSO _data;
    public void Setup(CardDataSO data, DeckManager deckManager)
    {
        _deckManager = deckManager;
        _data = data;
        _frontRenderer.sprite = data.FrontImage;
        _backRenderer.sprite = data.BackImage;
    }
    public async UniTask DrawFromDeck(Vector3 targetPosition, bool flip = true, float duration = 1f, float rotationsPerSecond = 2f)
    {
        int highSortOrder = 100;
        SetSortingOrder(highSortOrder);
        var originalRotation = transform.rotation;

        var moveTween = transform.DOMove(targetPosition, duration).SetEase(Ease.OutQuad);

        float spinDuration = Mathf.Max(0.001f, 1f / Mathf.Max(0.0001f, rotationsPerSecond));

        var rotateTween = transform.DOLocalRotate(new Vector3(0f, 0f, 360f), spinDuration, RotateMode.FastBeyond360)
            .SetRelative(true)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart);

        await moveTween.AsyncWaitForCompletion().AsUniTask();

        rotateTween.Kill();

        await transform.DORotateQuaternion(originalRotation, 0.25f).SetEase(Ease.OutQuad)
            .AsyncWaitForCompletion()
            .AsUniTask();
        if (flip)
        {
            await Flip();
        }
    }
    public async UniTask ReturnToDeck(float duration=1f)
    {
        await Flip();
        int highSortOrder = 100;
        SetSortingOrder(highSortOrder);
        var originalRotation = transform.rotation;
        var targetPosition = _deckManager.GetDeckPosition();

        var moveTween = transform.DOMove(targetPosition, duration).SetEase(Ease.OutQuad);

        await moveTween.AsyncWaitForCompletion().AsUniTask();
    }
    public async UniTask Move(Vector3 targetPosition, int sortingOrder, float duration = 0.25f)
    {
        SetSortingOrder(sortingOrder);
        var moveTween = transform.DOMove(targetPosition, duration).SetEase(Ease.OutQuad);
        await moveTween.AsyncWaitForCompletion().AsUniTask();
    }
    public async UniTask Flip()
    {
        IsFlipped = !IsFlipped;

        Quaternion targetRotation = IsFlipped ? _faceUpRotation : _faceDownRotation;
        await transform.DORotateQuaternion(targetRotation, 0.25f).SetEase(Ease.Linear).AsyncWaitForCompletion().AsUniTask();
    }
    public int GetValue()
    {
        return _data.CardValue;
    }
    public string GetName()
    {
        string result = $"{_data.CardSuit.ToString()}, {_data.CardRank.ToString()} ";
        return result;
    }
    private void SetSortingOrder(int sortingOrder)
    {
        _frontRenderer.sortingOrder = sortingOrder;
        _backRenderer.sortingOrder = sortingOrder;
    }
}
