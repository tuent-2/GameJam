using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PTT;
using Sirenix.OdinInspector;

public class Level_Balance_Tut_PressBtnBalance : ExtendMonoBehaviour
{
    //===================================================================== Variables
    [SerializeField] private GameObject _objHand;
    [SerializeField, DisableIn(PrefabKind.All)] private Level_Balance _levelBalance;
    [SerializeField] private OnScreenButton _btnBalance;
    private List<Ball> _subscribedBalls = new List<Ball>();
    //===================================================================== Unity Methods
    private void Awake()
    {
        if (_objHand != null)
        {
            _objHand.SetActive(false);
        }
    }

    private void Start()
    {
        StartCoroutine(SetLevelBalanceRefNextFrame());
        
        if (_btnBalance != null)
        {
            _btnBalance.OnActionMouseDown += HandleButtonBalancePressed;
        }
    }

    private IEnumerator SetLevelBalanceRefNextFrame()
    {
        yield return null; // Wait 1 frame after Start
        _levelBalance = GM.Instant?.GetModule<Level_Balance>();
        
        // Subscribe to all balls' OnStatusChanged events
        SubscribeToAllBalls();
    }

    private void OnDestroy()
    {
        // Unsubscribe from all balls
        UnsubscribeFromAllBalls();
        
        if (_btnBalance != null)
        {
            _btnBalance.OnActionMouseDown -= HandleButtonBalancePressed;
        }
    }
    //===================================================================== Action Handle
    private void HandleButtonBalancePressed()
    {
        if (_objHand != null)
        {
            _objHand.SetActive(false);
        }
    }

    private void HandleBallStatusChanged(EBallPositionStatus status)
    {
        CheckAllBallsInBalancer();
    }

    //===================================================================== Local Methods
    private void SubscribeToAllBalls()
    {
        if (_levelBalance == null || _levelBalance.listBallRef == null)
            return;

        UnsubscribeFromAllBalls(); // Clear previous subscriptions

        for (int i = 0; i < _levelBalance.listBallRef.Count; i++)
        {
            var ball = _levelBalance.listBallRef[i];
            if (ball == null) continue;

            ball.OnStatusChanged += HandleBallStatusChanged;
            _subscribedBalls.Add(ball);
        }
        
        // Check immediately after subscribing
        CheckAllBallsInBalancer();
    }

    private void UnsubscribeFromAllBalls()
    {
        for (int i = 0; i < _subscribedBalls.Count; i++)
        {
            var ball = _subscribedBalls[i];
            if (ball != null)
            {
                ball.OnStatusChanged -= HandleBallStatusChanged;
            }
        }
        _subscribedBalls.Clear();
    }

    private void CheckAllBallsInBalancer()
    {
        if (_levelBalance == null || _levelBalance.listBallRef == null || _objHand == null)
            return;

        if (_objHand.activeSelf)
            return; // Already active, no need to check again

        // Check if all balls are in balancer
        bool allBallsInBalancer = true;
        for (int i = 0; i < _levelBalance.listBallRef.Count; i++)
        {
            var ball = _levelBalance.listBallRef[i];
            if (ball == null) continue;

            if (ball.Status != EBallPositionStatus.OnBalancer)
            {
                allBallsInBalancer = false;
                break;
            }
        }

        if (allBallsInBalancer && _levelBalance.listBallRef.Count > 0)
        {
            _objHand.SetActive(true);
        }
    }
}
