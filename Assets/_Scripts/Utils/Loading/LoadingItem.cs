using System.Collections;
using DG.Tweening;
using UnityEngine;

public class LoadingItem : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float rotateSpeed = 200;
    [SerializeField] private float timeAutoStop = 15f;
    [SerializeField] private RectTransform iconLoading;
    private Coroutine _rotateIcon;
    private Coroutine _timeCount;
    private Tween _fadeOutTween;

    public void OnOpenLoading()
    {
        FadeInContent();
        if (_rotateIcon != null) StopCoroutine(_rotateIcon);
        if (_timeCount != null) StopCoroutine(_timeCount);
        _rotateIcon = StartCoroutine(IERotate());
        _timeCount = StartCoroutine(IETimer());
    }

    public void OnCloseLoading()
    {
        if (_rotateIcon != null) StopCoroutine(_rotateIcon);
        if (_timeCount != null) StopCoroutine(_timeCount);
        FadeOutContent();
    }

    private IEnumerator IERotate()
    {
        while (true)
        {
            iconLoading.Rotate(Vector3.forward * (rotateSpeed * Time.deltaTime));
            yield return null;
        }
    }

    private IEnumerator IETimer()
    {
        var t = timeAutoStop;
        while (t > 0)
        {
            t -= Time.deltaTime;
            yield return null;
        }

        FadeOutContent();
        Logger.Log("Lỗi server không phản hồi");
    }

    private void FadeInContent()
    {
        _fadeOutTween?.Kill();
        gameObject.SetActive(true);
        canvasGroup.DOFade(1, 0.2f);
    }

    private void FadeOutContent()
    {
        _fadeOutTween?.Kill();
        _fadeOutTween = canvasGroup.DOFade(0, 0.2f).OnComplete(() => { gameObject.SetActive(false); });
    }
}