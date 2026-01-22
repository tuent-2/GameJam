using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PTT;
using Sirenix.OdinInspector;

public class Balancer : ExtendMonoBehaviour
{
    //===================================================================== Variables
    [SerializeField, DisableIn(PrefabKind.All)] private float _fTotalWeight;
    [SerializeField, DisableIn(PrefabKind.All)] private List<Ball> _listBallOnWeightScale;
    [SerializeField] private ColliderContacter2D _contacter;
    //===================================================================== Unity Methods
    protected override void Awake()
    {
        base.Awake();
        if (_listBallOnWeightScale == null) _listBallOnWeightScale = new List<Ball>();
        if (_contacter != null)
        {
            _contacter.OnActionTriggerEnter += OnActionTriggerEnter;
            _contacter.OnActionTriggerExit += OnActionTriggerExit;
        }
    }

    private void OnDestroy()
    {
        if (_contacter == null) return;
        _contacter.OnActionTriggerEnter -= OnActionTriggerEnter;
        _contacter.OnActionTriggerExit -= OnActionTriggerExit;
    }

    //===================================================================== Action Handle

    private void OnActionTriggerEnter(Collider2D obj)
    {
        if (obj == null) return;
        if (_listBallOnWeightScale == null) _listBallOnWeightScale = new List<Ball>();

        Ball ball = obj.GetComponentInParent<Ball>();
        if (ball == null) return;

        // add + check duplicate
        if (!_listBallOnWeightScale.Contains(ball))
        {
            _listBallOnWeightScale.Add(ball);
        }
    }

    private void OnActionTriggerExit(Collider2D obj)
    {
        if (obj == null) return;
        if (_listBallOnWeightScale == null) return;

        Ball ball = obj.GetComponentInParent<Ball>();
        if (ball == null) return;

        // remove + check exist
        if (_listBallOnWeightScale.Contains(ball))
        {
            _listBallOnWeightScale.Remove(ball);
        }
    }

    //===================================================================== Local Methods

    public float GetTotalWeight()
    {
        if (_listBallOnWeightScale == null || _listBallOnWeightScale.Count == 0)
        {
            _fTotalWeight = 0f;
            return 0f;
        }

        float total = 0f;
        for (int i = 0; i < _listBallOnWeightScale.Count; i++)
        {
            var ball = _listBallOnWeightScale[i];
            if (ball == null) continue;
            var rb = ball.GetComponent<Rigidbody2D>();
            if (rb == null) continue;
            total += rb.mass;
        }

        _fTotalWeight = total;
        return total;
    }
}
