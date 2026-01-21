using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; // cần import DOTween

[RequireComponent(typeof(Toggle))]
public class ToggleEffect : MonoBehaviour
{
    [SerializeField] private Toggle tgEffect;
    [SerializeField] private AudioClip clickSound;


    [SerializeField] private ToggleAnimation animType = ToggleAnimation.No;

    [Header("If has no localize")] [ShowIf("animType", ToggleAnimation.Scale)] [SerializeField]
    private GameObject goIcon, goText;

    [SerializeField] private bool isDefault = false;


    private Vector3 _iconStartPos, _textStartPos;
    private Vector3 _iconStartScale, _textStartScale;

    private void OnValidate()
    {
        tgEffect ??= GetComponent<Toggle>();
    }

    private void Reset()
    {
        tgEffect = GetComponent<Toggle>();
    }

    private void Awake()
    {
        if (goIcon != null)
        {
            _iconStartPos = goIcon.transform.localPosition;
            _iconStartScale = goIcon.transform.localScale;
        }

        if (goText != null)
        {
            _textStartPos = goText.transform.localPosition;
            _textStartScale = goText.transform.localScale;
        }
    }

    private void Start()
    {
        tgEffect.onValueChanged.AddListener(isOn =>
        {
            if (isOn)
            {
                //AudioManager.PlaySound(clickSound);
                AnimateTarget(goIcon, _iconStartPos, _iconStartScale, true);
                //AnimateTarget(goText, _textStartPos, _textStartScale, true);
            }
            else
            {
                AnimateTarget(goIcon, _iconStartPos, _iconStartScale, false);
                //AnimateTarget(goText, _textStartPos, _textStartScale, false);
            }
        });

        // nếu isDefault thì set toggle mặc định và trigger animation
        if (isDefault)
        {
            tgEffect.isOn = true;
            AnimateTarget(goIcon, _iconStartPos, _iconStartScale, true);
            //AnimateTarget(goText, _textStartPos, _textStartScale, true);
        }
        else
        {
            tgEffect.isOn = false;
            AnimateTarget(goIcon, _iconStartPos, _iconStartScale, false);
            //AnimateTarget(goText, _textStartPos, _textStartScale, false);
        }
    }


    private void AnimateTarget(GameObject go, Vector3 startPos, Vector3 startScale, bool isOn)
    {
        if (go == null) return;

        go.transform.DOKill(); // hủy tween cũ để tránh chồng
        if (isOn)
        {
            go.transform.DOScale(startScale * 1.1f, 0.2f).SetEase(Ease.OutBack);
            go.transform.DOLocalMoveY(startPos.y + 25, 0.2f).SetEase(Ease.OutQuad);
        }
        else
        {
            go.transform.DOScale(startScale, 0.2f).SetEase(Ease.OutBack);
            go.transform.DOLocalMoveY(startPos.y, 0.2f).SetEase(Ease.OutQuad);
        }
    }

    public enum ToggleAnimation
    {
        No,
        Scale
    }
}