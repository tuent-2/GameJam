using DG.Tweening;
using TMPro;
using UnityEngine;

public static class DotweenExtension
{
    public static Tween DOIncreaseMoney(this TextMeshProUGUI text, float from, float to, float duration)
    {
        var tween = DOVirtual.Float(from, to, duration, value => { text.text = $"{((long)value).FormatMoney()}"; });
        return tween;
    }

    public static Tween DOIncreaseNumber(this TextMeshProUGUI text, string text2, float from, float to,
        float duration)
    {
        var tween = DOVirtual.Float(from, to, duration,
            value => { text.text = $"{((long)value).FormatMoney()}" + "%"; });
        return tween;
    }

    public static Tween DOMoveVerticalInLoop(this Transform transform, float moveDistance, float duration)
    {
        var tween = transform.DOLocalMoveY(moveDistance, duration)
            .SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear).OnKill(() => { transform.localPosition = Vector3.zero; });
        return tween;
    }
}