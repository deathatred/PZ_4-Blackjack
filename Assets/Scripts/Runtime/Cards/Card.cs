using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _frontRenderer;
    [SerializeField] private SpriteRenderer _backRenderer;

    private CardDataSO _data;
    public void Setup(CardDataSO data)
    {
        _data = data;
        _frontRenderer.sprite = data.FrontImage;
        _backRenderer.sprite = data.BackImage;
    }
    public async UniTask DrawFromDeck(Vector3 targetPosition, bool flip = true, float duration = 1f, float rotationsPerSecond = 2f)
    {
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
            var flipRotation = Quaternion.Euler(90f, 180f, -180);
            await transform.DORotateQuaternion(flipRotation, 0.25f).SetEase(Ease.Linear).AsyncWaitForCompletion().AsUniTask();
        }
    }
    public async UniTask Move(Vector3 targetPosition, float duration = 0.25f)
    {
        var moveTween = transform.DOMove(targetPosition, duration).SetEase(Ease.OutQuad);
        await moveTween.AsyncWaitForCompletion().AsUniTask();
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
}
