using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PTT;
using Sirenix.OdinInspector;
using TMPro;

public enum EBallPositionStatus
{
    Prepare = 0,
    OnBalancer = 1,
    OnAnswerZone = 2,
    NoWhere = 3,
}

public class Ball : ExtendMonoBehaviour
{
    //===================================================================== Variables

    [SerializeField, OnValueChanged(nameof(HandleShowTextWeightChanged))] private bool _bShowTextWeight = true;
    [SerializeField, OnValueChanged(nameof(HandleWeightChanged))] private bool _bHideWeight;
    [SerializeField, OnValueChanged(nameof(HandleWeightChanged))] private bool _bFakeWeight;
    [SerializeField, ShowIf("@_bFakeWeight"), OnValueChanged(nameof(HandleWeightChanged))] 
    private float _fFakeWeight;

    [SerializeField, DisableIn(PrefabKind.All)] 
    private EBallPositionStatus _status = EBallPositionStatus.NoWhere;
    
    public Action<EBallPositionStatus> OnStatusChanged;
    
    [SerializeField, OnValueChanged(nameof(HandleWeightChanged))] private float _fWeight;
    [SerializeField] private Color _colorNormal;
    [SerializeField] private Color _colorTrue;
    [SerializeField] private Color _colorFail;

    [SerializeField] private Rigidbody2D _rb2D;
    [SerializeField] private ColliderContacter2D _coll;
    [SerializeField] private TextMeshPro _tmpWeight;

    public float Weight => _fWeight;
    public float DisplayWeight => _bFakeWeight ? _fFakeWeight : _fWeight;
    public EBallPositionStatus Status => _status;
    
    [Header("Drag 2D")]
    [SerializeField] private bool _enableDrag = true;
    [SerializeField,DisableIn(PrefabKind.All)] private Camera _cam;

    private Collider2D _col2D;
    private bool _dragging;
    private Vector3 _dragOffset;
    private Vector2 _dragTarget;
    //===================================================================== Unity Methods
    protected override void Awake()
    {
        base.Awake();
        if (_rb2D == null) _rb2D = GetComponent<Rigidbody2D>();
        if (_coll == null) _coll = GetComponent<ColliderContacter2D>();
        if (_cam == null) _cam = Camera.main;
        if (_cam == null) _cam = FindObjectOfType<Camera>();
        _col2D = GetComponent<Collider2D>();

        if (_rb2D == null)
        {
            Debug.LogError($"{nameof(Ball)} needs a {nameof(Rigidbody2D)}.", this);
            return;
        }

        HandleWeightChanged();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        HandleWeightChanged();
    }
#endif

    private void OnEnable()
    {
        if (_coll == null) _coll = GetComponent<ColliderContacter2D>();
        if (_coll == null) return;

        _coll.OnActionTriggerEnter += HandleTriggerEnter;
        _coll.OnActionTriggerExit += HandleTriggerExit;
    }

    private void OnDisable()
    {
        if (_coll == null) return;

        _coll.OnActionTriggerEnter -= HandleTriggerEnter;
        _coll.OnActionTriggerExit -= HandleTriggerExit;
    }

    private void Update()
    {
        if (!_enableDrag) return;
        if (_rb2D == null) return;
        if (_col2D == null) _col2D = GetComponent<Collider2D>();
        if (_col2D == null) return; // cần collider để bắt click
        if (_cam == null) _cam = Camera.main;
        if (_cam == null) _cam = FindObjectOfType<Camera>();
        if (_cam == null) return;

        // Mouse (legacy input). Không dùng OnMouseDown để tránh fail khi project dùng New Input System-only.
        if (Input.GetMouseButtonDown(0))
        {
            var world = ScreenToWorld(Input.mousePosition);
            if (_col2D.OverlapPoint(world))
            {
                _dragging = true;
                _dragOffset = transform.position - world;
                UpdateDragTarget(world);
            }
        }
        else if (_dragging && Input.GetMouseButton(0))
        {
            UpdateDragTarget(ScreenToWorld(Input.mousePosition));
        }
        else if (_dragging && Input.GetMouseButtonUp(0))
        {
            _dragging = false;
        }

        // Touch (legacy)
        if (Input.touchCount > 0)
        {
            var t = Input.GetTouch(0);
            var world = ScreenToWorld(t.position);
            if (t.phase == TouchPhase.Began)
            {
                if (_col2D.OverlapPoint(world))
                {
                    _dragging = true;
                    _dragOffset = transform.position - world;
                    UpdateDragTarget(world);
                }
            }
            else if (_dragging && (t.phase == TouchPhase.Moved || t.phase == TouchPhase.Stationary))
            {
                UpdateDragTarget(world);
            }
            else if (_dragging && (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled))
            {
                _dragging = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (!_enableDrag) return;
        if (!_dragging) return;
        if (_rb2D == null) return;

#if UNITY_6000_0_OR_NEWER
        _rb2D.linearVelocity = Vector2.zero;
#else
        _rb2D.velocity = Vector2.zero;
#endif
        _rb2D.angularVelocity = 0f;

        if (_rb2D.bodyType == RigidbodyType2D.Static)
        {
            transform.position = new Vector3(_dragTarget.x, _dragTarget.y, transform.position.z);
        }
        else
        {
            _rb2D.MovePosition(_dragTarget);
        }
    }
    //===================================================================== Action Handle

    //===================================================================== Local Methods
    private void HandleShowTextWeightChanged()
    {
        if (_tmpWeight == null) return;

        _tmpWeight.gameObject.SetActive(_bShowTextWeight);

        // Refresh label right away when re-enabled in Inspector.
        if (_bShowTextWeight) HandleWeightChanged();
    }

    private void HandleWeightChanged()
    {
        if (_rb2D == null) _rb2D = GetComponent<Rigidbody2D>();
        if (_rb2D != null) _rb2D.mass = _fWeight;

        if (_tmpWeight == null) return;

        // Allow toggling weight label on/off via Inspector (Odin OnValueChanged).
        if (!_bShowTextWeight)
        {
            if (_tmpWeight.gameObject.activeSelf) _tmpWeight.gameObject.SetActive(false);
            return;
        }

        if (!_tmpWeight.gameObject.activeSelf) _tmpWeight.gameObject.SetActive(true);

        if (_bHideWeight)
        {
            _tmpWeight.text = "?";
            return;
        }

        _tmpWeight.text = (_bFakeWeight ? _fFakeWeight : _fWeight).ToString("0.##");
    }

    private Vector3 ScreenToWorld(Vector2 screenPos)
    {
        // Convert screen->world on the plane of this object (works for ortho + perspective).
        float z = Mathf.Abs(_cam.transform.position.z - transform.position.z);
        Vector3 wp = _cam.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, z));
        wp.z = transform.position.z;
        return wp;
    }

    private void UpdateDragTarget(Vector3 worldPoint)
    {
        Vector3 target = worldPoint + _dragOffset;
        _dragTarget = new Vector2(target.x, target.y);
    }

    private void HandleTriggerEnter(Collider2D other)
    {
        if (other == null) return;

        TagGameForDetect tag = null;
        if (!other.TryGetComponent(out tag)) tag = other.GetComponentInParent<TagGameForDetect>();
        if (tag == null) return;

        var eTag = tag.eTag;
        switch (eTag)
        {
            case ETagGameForDetect.PrepareZone:
                SetStatus(EBallPositionStatus.Prepare);
                break;
            case ETagGameForDetect.BalanceZone:
                SetStatus(EBallPositionStatus.OnBalancer);
                break;
            case ETagGameForDetect.AnswerZone:
                SetStatus(EBallPositionStatus.OnAnswerZone);
                break;
            case ETagGameForDetect.Ball:
                // ignore
                break;
            default:
                SetStatus(EBallPositionStatus.NoWhere);
                break;
        }
    }

    private void HandleTriggerExit(Collider2D other)
    {
        if (other == null) return;
        
        SetStatus(EBallPositionStatus.NoWhere);
    }
    
    private void SetStatus(EBallPositionStatus newStatus)
    {
        if (_status != newStatus)
        {
            _status = newStatus;
            OnStatusChanged?.Invoke(_status);
        }
    }
}
