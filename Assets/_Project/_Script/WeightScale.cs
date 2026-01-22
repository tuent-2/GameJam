using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PTT;
using DG.Tweening;
using Sirenix.OdinInspector;

public enum EWeightScaleStatus
{
    Balance = 0,
    LeftHeavier = 1,
    RightHeavier = 2
}

public class WeightScale : ExtendMonoBehaviour
{
    //===================================================================== Variables
    [SerializeField] private Balancer _balancerLeft;
    [SerializeField] private Balancer _balancerRight;
    [SerializeField] private OnScreenButton _onScreenButton;
    [SerializeField] private float _moveOffsetY = 0.5f; // x: lệch lên/xuống theo trục Y
    [SerializeField] private float _tweenDuration = 0.25f;
    [SerializeField] private float _equalEpsilon = 0.0001f;

    public float TweenDuration => _tweenDuration;
    
    public EWeightScaleStatus eWeightScaleStartStatus;
    [DisableIn(PrefabKind.All)] public  EWeightScaleStatus _status = EWeightScaleStatus.Balance;
    
    private Vector3 _leftBaseLocalPos;
    private Vector3 _rightBaseLocalPos;

    private S_GM_Event _gmEvent;
    private Level _level;
    private Coroutine _initRoutine;
    //===================================================================== Unity Methods
    protected override void Awake()
    {
        base.Awake();
        if (_balancerLeft != null) _leftBaseLocalPos = _balancerLeft.transform.localPosition;
        if (_balancerRight != null) _rightBaseLocalPos = _balancerRight.transform.localPosition;

        if (_onScreenButton != null) _onScreenButton.OnActionMouseDown += OnActionMouseDown;
        else DebugLog($"{nameof(WeightScale)} missing OnScreenButton reference.");
    }

    private void Start()
    {
        // GM registers modules in its Awake, so Start is safer to resolve.
        _gmEvent = GM.Instant.GetModule<S_GM_Event>();
        _level = GM.Instant.GetModule<Level>();

        // 1 frame after Start: apply initial status + refresh balancer visuals.
        if (_initRoutine != null) StopCoroutine(_initRoutine);
        _initRoutine = StartCoroutine(InitAfterOneFrame());
    }

    private void OnDestroy()
    {
        if (_onScreenButton != null) _onScreenButton.OnActionMouseDown -= OnActionMouseDown;
        if (_initRoutine != null) StopCoroutine(_initRoutine);
    }

    //===================================================================== Action Handle

    private void OnActionMouseDown()
    {
        if (_balancerLeft == null || _balancerRight == null)
        {
            DebugLog($"{nameof(WeightScale)} missing balancer reference.");
            return;
        }

        // Consume 1 try each time player triggers the scale action.
        _level ??= GM.Instant.GetModule<Level>();
        if (_level != null)
        {
            _level.iCurrentTryAmount = Mathf.Max(0, _level.iCurrentTryAmount - 1);
        }

        float leftW = _balancerLeft.GetTotalWeight();
        float rightW = _balancerRight.GetTotalWeight();
        float diff = leftW - rightW;
        DebugLog($"Computed weights: left={leftW}, right={rightW}, diff={diff}, x={_moveOffsetY}, eps={_equalEpsilon}");

        // Update status first, then broadcast.
        EWeightScaleStatus newStatus =
            Mathf.Abs(diff) <= _equalEpsilon ? EWeightScaleStatus.Balance :
            (diff > 0f ? EWeightScaleStatus.LeftHeavier : EWeightScaleStatus.RightHeavier);

        _status = newStatus;
        RaiseWeightScaleStatusChanged(_status);

        if (_status == EWeightScaleStatus.Balance)
        {
            DebugLog("bằng");
            TweenBalancer(_balancerLeft.transform, _leftBaseLocalPos.y);
            TweenBalancer(_balancerRight.transform, _rightBaseLocalPos.y);
            return;
        }

        bool leftHeavier = diff > 0f;
        float heavierY = (leftHeavier ? _leftBaseLocalPos.y : _rightBaseLocalPos.y) - _moveOffsetY;
        float lighterY = (leftHeavier ? _rightBaseLocalPos.y : _leftBaseLocalPos.y) + _moveOffsetY;

        DebugLog($"Lệch: {(leftHeavier ? "Left" : "Right")} nặng hơn. left={leftW}, right={rightW}, diff={diff}, heavierY={heavierY}, lighterY={lighterY}");

        if (leftHeavier)
        {
            TweenBalancer(_balancerLeft.transform, heavierY);
            TweenBalancer(_balancerRight.transform, lighterY);
        }
        else
        {
            TweenBalancer(_balancerRight.transform, heavierY);
            TweenBalancer(_balancerLeft.transform, lighterY);
        }
    }
    
    //===================================================================== Local Methods
    private IEnumerator InitAfterOneFrame()
    {
        // Wait 1 frame so other Start() logic can finish first.
        yield return null;

        if (_balancerLeft == null || _balancerRight == null)
        {
            DebugLog($"{nameof(WeightScale)} missing balancer reference.");
            yield break;
        }

        // Refresh base positions (in case they were adjusted on the first frame).
        _leftBaseLocalPos = _balancerLeft.transform.localPosition;
        _rightBaseLocalPos = _balancerRight.transform.localPosition;

        ApplyStatus(eWeightScaleStartStatus, broadcast: true);
    }

    private void ApplyStatus(EWeightScaleStatus status, bool broadcast)
    {
        _status = status;
        if (broadcast) RaiseWeightScaleStatusChanged(_status);

        if (_balancerLeft == null || _balancerRight == null) return;

        if (_status == EWeightScaleStatus.Balance)
        {
            TweenBalancer(_balancerLeft.transform, _leftBaseLocalPos.y);
            TweenBalancer(_balancerRight.transform, _rightBaseLocalPos.y);
            return;
        }

        if (_status == EWeightScaleStatus.LeftHeavier)
        {
            TweenBalancer(_balancerLeft.transform, _leftBaseLocalPos.y - _moveOffsetY);
            TweenBalancer(_balancerRight.transform, _rightBaseLocalPos.y + _moveOffsetY);
            return;
        }

        // RightHeavier
        TweenBalancer(_balancerRight.transform, _rightBaseLocalPos.y - _moveOffsetY);
        TweenBalancer(_balancerLeft.transform, _leftBaseLocalPos.y + _moveOffsetY);
    }

    private void RaiseWeightScaleStatusChanged(EWeightScaleStatus status)
    {
        // In case Start hasn't run yet or module list changed.
        _gmEvent ??= GM.Instant.GetModule<S_GM_Event>();
        _gmEvent?.OnWeightScaleStatusChange?.Invoke(status);
    }

    private void TweenBalancer(Transform t, float targetLocalY)
    {
        if (t == null) return;
        t.DOKill();
        t.DOLocalMoveY(targetLocalY, _tweenDuration).SetEase(Ease.OutQuad);
    }
}
