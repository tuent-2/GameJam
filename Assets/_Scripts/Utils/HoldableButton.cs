using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoldableButton : Button
{
    private Coroutine _holdingCoroutine;

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        _holdingCoroutine = StartCoroutine(IEHoldButton());
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        if (_holdingCoroutine != null) StopCoroutine(_holdingCoroutine);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (_holdingCoroutine != null) StopCoroutine(_holdingCoroutine);
    }

    private IEnumerator IEHoldButton()
    {
        var delayTime = 0.2f;
        yield return Helpers.GetWaitForSeconds(0.2f);
        while (isActiveAndEnabled)
        {
            onClick?.Invoke();
            yield return Helpers.GetWaitForSeconds(delayTime);
            if (delayTime > 0.02f)
                delayTime -= 0.02f;
        }
    }
}