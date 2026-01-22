using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PTT;
using DG.Tweening;

public class OnScreenButton : ExtendMonoBehaviour
{
    //===================================================================== Variables
    public Action OnActionMouseDown;
    private bool _canPress = true;
    private Vector3 _originalScale;
    //===================================================================== Unity Methods
    private void Start()
    {
        _originalScale = transform.localScale;
    }
    
    private void OnMouseDown()
    {
        if (!_canPress) return;
        
        // Chỉ cho bấm khi game state là Playing
        GM_State gmState = GM.Instant?.GetModule<GM_State>();
        if (gmState == null || gmState.eGameStateCurrent != EGameState.Playing) return;
        
        // Hiệu ứng DoScale khi bấm
        transform.DOScale(_originalScale * 0.9f, 0.1f)
            .OnComplete(() => {
                transform.DOScale(_originalScale, 0.1f);
            });
        
        OnActionMouseDown?.Invoke();
        
        // Delay 1s trước khi cho bấm lại
        StartCoroutine(DisableButtonTemporarily());
    }
    //===================================================================== Action Handle
    
    //===================================================================== Local Methods
    private IEnumerator DisableButtonTemporarily()
    {
        _canPress = false;
        yield return new WaitForSeconds(1f);
        _canPress = true;
    }
}
