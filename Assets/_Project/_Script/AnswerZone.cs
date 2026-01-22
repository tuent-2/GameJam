using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PTT;
using Sirenix.OdinInspector;

public class AnswerZone : ExtendMonoBehaviour
{
    //===================================================================== Variables
    [SerializeField] private OnScreenButton _btn;
    [SerializeField] private ColliderContacter2D _contacter2D;
    [SerializeField,DisableIn(PrefabKind.All)] 
    private Ball _ballCurrent;
    //===================================================================== Unity Methods
    protected override void Awake()
    {
        base.Awake();
        
        if (_contacter2D != null)
        {
            _contacter2D.OnActionTriggerEnter += HandleTriggerEnter;
            _contacter2D.OnActionTriggerExit += HandleTriggerExit;
        }
        
        if (_btn != null)
        {
            _btn.OnActionMouseDown += HandleButtonMouseDown;
        }
    }

    private void OnDestroy()
    {
        if (_contacter2D != null)
        {
            _contacter2D.OnActionTriggerEnter -= HandleTriggerEnter;
            _contacter2D.OnActionTriggerExit -= HandleTriggerExit;
        }
        
        if (_btn != null)
        {
            _btn.OnActionMouseDown -= HandleButtonMouseDown;
        }
    }

    //===================================================================== Action Handle
    private void HandleButtonMouseDown()
    {
        Level_FindOne levelFindOne = FindObjectOfType<Level_FindOne>();
        if (levelFindOne == null) return;
        
        GM_State gmState = GM.Instant?.GetModule<GM_State>();
        if (gmState == null) return;
        
        if (_ballCurrent == levelFindOne.ballAnswer)
        {
            gmState.eGameStateCurrent = EGameState.EndWin;
        }
        else
        {
            gmState.eGameStateCurrent = EGameState.EndLose;
        }
    }

    //===================================================================== Local Methods
    private void HandleTriggerEnter(Collider2D other)
    {
        if (other == null) return;
        
        TagGameForDetect tag = null;
        if (!other.TryGetComponent(out tag)) tag = other.GetComponentInParent<TagGameForDetect>();
        if (tag == null) return;
        
        if (tag.eTag != ETagGameForDetect.Ball) return;
        
        Ball ball = other.GetComponent<Ball>();
        if (ball == null) ball = other.GetComponentInParent<Ball>();
        if (ball == null) return;
        
        _ballCurrent = ball;
    }

    private void HandleTriggerExit(Collider2D other)
    {
        if (other == null) return;
        
        TagGameForDetect tag = null;
        if (!other.TryGetComponent(out tag)) tag = other.GetComponentInParent<TagGameForDetect>();
        if (tag == null) return;
        
        if (tag.eTag != ETagGameForDetect.Ball) return;
        
        Ball ball = other.GetComponent<Ball>();
        if (ball == null) ball = other.GetComponentInParent<Ball>();
        if (ball == null) return;
        
        if (_ballCurrent == ball)
        {
            _ballCurrent = null;
        }
    }
}
