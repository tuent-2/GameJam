using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class ToastItem : MonoBehaviour
{
    [SerializeField] private CanvasGroup @group;
    [SerializeField] private LocalizeText toastTxt;
    [SerializeField] private RectTransform rectTrs;

    private Sequence _moveSeq;
    private float _showPosY;
    private float _timeShow;
    public const float TIME_APPEAR = 0.2f;
    public const float TIME_DISAPPEAR = 0.1f;

    private void OnEnable()
    {
        Toast.OnAddToast.Dispatch(this);
    }

    private void OnDisable()
    {
        Toast.OnRemoveToast.Dispatch(this);
    }

    public ToastItem SetTimeShow(float t)
    {
        _timeShow = t;
        return this;
    }

    public ToastItem SetStartPosY(float pos)
    {
        _showPosY = pos;
        return this;
    }

    public ToastItem SetText(string str, string prependText, string appendText)
    {
        toastTxt.UpdateStringReferenceByKey(str, prependText: prependText, appendText: appendText);
        return this;
    }

    public ToastItem SetStringText(string str)
    {
        toastTxt.UpdateStringText(str);
        return this;
    }

    public void ShowToast()
    {
        rectTrs.ChangeAnchorY(_showPosY - 100);
        rectTrs.localScale =
            Screen.orientation is (ScreenOrientation.Portrait or ScreenOrientation.PortraitUpsideDown)
                ? Vector3.one
                : Vector3.one;
        @group.alpha = 0;
        PlayAnim();
    }

    private void PlayAnim()
    {
        _moveSeq?.Kill();
        group.alpha = 0;
        _moveSeq = DOTween.Sequence().Append(@group.DOFade(1, 0.2f))
            .Join(rectTrs.DOAnchorPosY(_showPosY, TIME_APPEAR))
            .AppendInterval(_timeShow)
            .Append(rectTrs.DOAnchorPosY(_showPosY + 50, TIME_DISAPPEAR))
            .Join(@group.DOFade(0, TIME_DISAPPEAR))
            .OnComplete(() => { Pool.Release(this); });
    }
}