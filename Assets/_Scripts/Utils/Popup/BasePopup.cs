using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup), typeof(Canvas), typeof(GraphicRaycaster))]
public abstract class BasePopup<T> : BasePopup where T : BasePopup<T>
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Canvas canvas;
    [SerializeField] protected Transform content;
    [SerializeField] private int showLayer = 1;

    [SerializeField] private MoveType moveType;

    [Header("Scale Animation Attributes")] [SerializeField]
    private Ease openEase = Ease.OutBack;

    [SerializeField] private Ease closeEase = Ease.InSine;

    [SerializeField] private float openTime = 0.3f;
    [SerializeField] private float closeTime = 0.3f;

    [SerializeField] private float openScale = 0.4f;
    [SerializeField] private float closeScale = 0.4f;


    [Header("Move Animation Attributes")] [SerializeField]
    private float moveOffset = 1000f;

    public override bool IsOpening { get; set; }
    private Sequence _openCloseSequence;

    private void OnValidate()
    {
        canvasGroup ??= GetComponent<CanvasGroup>();
        canvas ??= GetComponent<Canvas>();
    }

    public static void Open(Action<T> onOpen = null)
    {
        //FirebaseAnalyticsUtils.Instance.LogEvent($"Open_{typeof(T).Name}_Popup");
        PopupManager.Open(onOpen);
    }

    public static void OpenCreateNew(Action<T> onOpen = null)
    {
        //FirebaseAnalyticsUtils.Instance.LogEvent($"Open_{typeof(T).Name}_Popup");
        PopupManager.OpenCreateNew(onOpen);
    }


    public static void Close()
    {
        //FirebaseAnalyticsUtils.Instance.LogEvent($"Close_{typeof(T).Name}_Popup");
        PopupManager.Close<T>();
    }

    public static bool IsOpen()
    {
        return PopupManager.IsOpen<T>();
    }

    public override void OnOpen()
    {
        IsOpening = true;
        gameObject.SetActive(true);
        canvas.overrideSorting = true;
        canvas.sortingOrder = showLayer + 1;
        canvasGroup.alpha = 0f;
        _openCloseSequence?.Kill();
        _openCloseSequence = DOTween.Sequence();


        switch (moveType)
        {
            case MoveType.AppearFromLeft:
                SideAnimationPopupOpen(-moveOffset, true);
                break;
            case MoveType.AppearFromRight:
                SideAnimationPopupOpen(moveOffset, true);
                break;
            case MoveType.AppearFromBottom:
                SideAnimationPopupOpen(-moveOffset, false);
                break;
            case MoveType.AppearFromTop:
                SideAnimationPopupOpen(moveOffset, false);
                break;
            case MoveType.Scale:
                content.localScale = Vector3.one * openScale;
                canvasGroup.alpha = 0f;
                _openCloseSequence.Append(content.DOScale(Vector3.one, openTime).SetEase(openEase));
                break;
            default:
                break;
        }


        _openCloseSequence.Join(canvasGroup.DOFade(1f, openTime));
    }

    private void SideAnimationPopupOpen(float moveValue, bool isMoveHorizontal)
    {
        var localPosition = content.localPosition;
        localPosition = isMoveHorizontal
            ? new Vector3(localPosition.x + moveValue, localPosition.y, localPosition.z)
            : new Vector3(localPosition.x, localPosition.y + moveValue, localPosition.z);

        content.localPosition = localPosition;
        _openCloseSequence.Append(isMoveHorizontal
            ? ((RectTransform)content).DOAnchorPosX(0, openTime).SetEase(openEase)
            : ((RectTransform)content).DOAnchorPosY(0, openTime).SetEase(openEase));
    }

    private void SideAnimationPopupClose(float moveValue, bool isMoveHorizontal)
    {
        _openCloseSequence.Append(isMoveHorizontal
            ? ((RectTransform)content).DOAnchorPosX(moveValue, closeTime).SetEase(closeEase)
            : ((RectTransform)content).DOAnchorPosY(moveValue, closeTime).SetEase(closeEase));
    }

    public override void OnClose()
    {
        IsOpening = false;
        _openCloseSequence?.Kill();
        _openCloseSequence = DOTween.Sequence();

        switch (moveType)
        {
            case MoveType.AppearFromLeft:
                SideAnimationPopupClose(0, true);
                break;
            case MoveType.AppearFromRight:
                SideAnimationPopupClose(moveOffset, true);
                break;
            case MoveType.AppearFromBottom:
                SideAnimationPopupClose(0, false);
                break;
            case MoveType.AppearFromTop:
                SideAnimationPopupClose(moveOffset, false);
                break;
            case MoveType.Scale:
                _openCloseSequence.Append(content.DOScale(Vector3.one * closeScale, closeTime).SetEase(closeEase));
                break;
            default:
                break;
        }

        _openCloseSequence.Join(canvasGroup.DOFade(0f, closeTime));
        _openCloseSequence.OnComplete(() => { gameObject.SetActive(false); });
    }

    private enum MoveType
    {
        Scale,
        AppearFromTop,
        AppearFromLeft,
        AppearFromRight,
        AppearFromBottom
    }
}


public abstract class BasePopup : MonoBehaviour
{
    public abstract bool IsOpening { get; set; }
    public abstract void OnOpen();
    public abstract void OnClose();
}