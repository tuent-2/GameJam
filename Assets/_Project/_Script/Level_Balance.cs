using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PTT;

public class Level_Balance : Level
{
    //===================================================================== Variables
    private bool _resolveResultOnNextWeightScaleStatus;
    private bool _resolvedResult;
    private Coroutine _resolveRoutine;
    private WeightScale _weightScale;

    //===================================================================== Unity Methods
    protected override void Start()
    {
        base.Start();
        StartCoroutine(SubscribeEventsNextFrame());
    }

    private IEnumerator SubscribeEventsNextFrame()
    {
        yield return null; // wait 1 frame after Start
        GM.Instant.GetModule<S_GM_Event>().OnWeightScaleStatusChange += OnWeightScaleStatusChange;

        // Cache WeightScale reference (optional, used for tween duration).
        _weightScale = FindObjectOfType<WeightScale>(true);
    }

    private void OnDestroy()
    {
        if (_resolveRoutine != null) StopCoroutine(_resolveRoutine);
        _resolveRoutine = null;

        var gmEvent = _gmEvent ?? GM.Instant.GetModule<S_GM_Event>();
        if (gmEvent != null)
        {
            gmEvent.OnWeightScaleStatusChange -= OnWeightScaleStatusChange;
        }
    }
    
    private void OnWeightScaleStatusChange(EWeightScaleStatus status)
    {
        // Allow win immediately when status is Balance (even if try amount > 0).
        // For lose conditions, only resolve when try amount reached 0.
        bool isWinCondition = status == EWeightScaleStatus.Balance;
        bool canResolve = isWinCondition || _resolveResultOnNextWeightScaleStatus;
        
        if (!canResolve || _resolvedResult) return;

        _resolveResultOnNextWeightScaleStatus = false;
        _resolvedResult = true;

        if (_resolveRoutine != null) StopCoroutine(_resolveRoutine);
        _resolveRoutine = StartCoroutine(ResolveResultAfterTweenAndDelay(status));
    }

    protected override void HandleCurrentTryAmountChange(int currentTryAmount)
    {
        if (currentTryAmount > 0) return;

        // When try amount reaches 0, resolve the result on the next weight-scale status event (exactly once).
        _resolveResultOnNextWeightScaleStatus = true;
    }
    
    //===================================================================== Action Handle

    //===================================================================== Local Methods
    private IEnumerator ResolveResultAfterTweenAndDelay(EWeightScaleStatus status)
    {
        // WeightScale broadcasts status BEFORE starting DOTween, so we wait for tween duration + 1s.
        _weightScale ??= FindObjectOfType<WeightScale>(true);
        float tweenDuration = _weightScale != null ? _weightScale.TweenDuration : 0f;
        yield return new WaitForSeconds(tweenDuration + 0.1f);

        var gmState = _gmState ?? GM.Instant.GetModule<S_GM_State>();
        if (gmState == null) yield break;

        // Requirement:
        // - If status == Balance => EndLose
        // - Else => EndWin
        gmState.eGameStateCurrent =
            status == EWeightScaleStatus.Balance ? EGameState.EndWin : EGameState.EndLose;
    }
}
