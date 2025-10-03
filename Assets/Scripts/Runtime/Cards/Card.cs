using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
using UnityEngine;
using Zenject;

public class Card : MonoBehaviour
{
    [Inject] private DeckManager _deckManager;
    [SerializeField] private SpriteRenderer _frontRenderer;
    [SerializeField] private SpriteRenderer _backRenderer;

    private CancellationTokenSource _cts = new CancellationTokenSource();

    public bool IsFlipped { get; private set; } = false;
    private Quaternion _faceDownRotation = Quaternion.Euler(-90f, 0f, -180f);
    private Quaternion _faceUpRotation = Quaternion.Euler(90f, 180f, -180f);
    private CardDataSO _data;

    private void OnDestroy()
    {
        _cts.Cancel();                 
        DOTween.Kill(gameObject);      
    }

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

        var moveTween = transform.DOMove(targetPosition, duration)
                                 .SetEase(Ease.OutQuad)
                                 .SetTarget(gameObject);

        float spinDuration = Mathf.Max(0.001f, 1f / Mathf.Max(0.0001f, rotationsPerSecond));

        var rotateTween = transform.DOLocalRotate(new Vector3(0f, 0f, 360f), spinDuration, RotateMode.FastBeyond360)
                                  .SetRelative(true)
                                  .SetEase(Ease.Linear)
                                  .SetLoops(-1, LoopType.Restart)
                                  .SetTarget(gameObject);

        await moveTween.AsyncWaitForCompletion().AsUniTask().AttachExternalCancellation(_cts.Token);

        rotateTween.Kill();

        await transform.DORotateQuaternion(originalRotation, 0.25f)
                       .SetEase(Ease.OutQuad)
                       .AsyncWaitForCompletion().AsUniTask()
                       .AttachExternalCancellation(_cts.Token);

        if (flip)
        {
            await Flip();
        }
    }

    public async UniTask ReturnToDeck(float duration = 1f)
    {
        await Flip();

        int highSortOrder = 100;
        SetSortingOrder(highSortOrder);
        var originalRotation = transform.rotation;
        var targetPosition = _deckManager.GetDeckPosition();

        var moveTween = transform.DOMove(targetPosition, duration)
                                 .SetEase(Ease.OutQuad)
                                 .SetTarget(gameObject);

        await moveTween.AsyncWaitForCompletion().AsUniTask().AttachExternalCancellation(_cts.Token);
    }

    public async UniTask Move(Vector3 targetPosition, int sortingOrder, float duration = 0.25f)
    {
        SetSortingOrder(sortingOrder);

        var moveTween = transform.DOMove(targetPosition, duration)
                                 .SetEase(Ease.OutQuad)
                                 .SetTarget(gameObject);

        await moveTween.AsyncWaitForCompletion().AsUniTask().AttachExternalCancellation(_cts.Token);
    }

    public async UniTask Flip()
    {
        IsFlipped = !IsFlipped;
        Quaternion targetRotation = IsFlipped ? _faceUpRotation : _faceDownRotation;

        var rotateTween = transform.DORotateQuaternion(targetRotation, 0.25f)
                                  .SetEase(Ease.Linear)
                                  .SetTarget(gameObject);

        await rotateTween.AsyncWaitForCompletion().AsUniTask().AttachExternalCancellation(_cts.Token);
    }

    public int GetValue() => _data.CardValue;

    public string GetName() => $"{_data.CardSuit}, {_data.CardRank}";

    private void SetSortingOrder(int sortingOrder)
    {
        _frontRenderer.sortingOrder = sortingOrder;
        _backRenderer.sortingOrder = sortingOrder;
    }
}
