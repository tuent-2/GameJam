using System.Collections;
using UnityEngine;
using PTT;
using TMPro;
using DG.Tweening;

public class Condition : ExtendMonoBehaviour
{
    //===================================================================== Variables
    [SerializeField] private TextMeshPro _tmp;

    private S_GM_Event _gmEvent;
    private Color _originalColor;
    private float _originalFontSize;
    private bool _isInitialized;
    private int _tweenId;

    //===================================================================== Unity Methods
    private void Awake()
    {
        // Generate unique ID for this instance's tweens
        _tweenId = GetInstanceID();
    }

    private void Start()
    {
        StartCoroutine(SubscribeEventsNextFrame());
    }

    private IEnumerator SubscribeEventsNextFrame()
    {
        yield return null; // 1 frame after Start

        // Initialize original color and font size
        if (_tmp != null && !_isInitialized)
        {
            _originalColor = _tmp.color;
            _originalFontSize = _tmp.fontSize;
            _isInitialized = true;
        }

        _gmEvent = GM.Instant?.GetModule<S_GM_Event>();
        if (_gmEvent != null)
        {
            _gmEvent.OnCurrentTryAmountChange += HandleCurrentTryAmountChange;
        }

        // Best-effort initial sync so UI is correct before the first event.
        var level = GM.Instant?.GetModule<Level>();
        if (level != null)
        {
            HandleCurrentTryAmountChange(level.iCurrentTryAmount);
        }
    }

    private void OnDestroy()
    {
        if (_gmEvent != null)
        {
            _gmEvent.OnCurrentTryAmountChange -= HandleCurrentTryAmountChange;
        }
        _gmEvent = null;

        // Kill only this instance's tweens using unique ID
        DOTween.Kill(_tweenId);
    }

    //===================================================================== Action Handle
    private void HandleCurrentTryAmountChange(int currentTryAmount)
    {
        if (_tmp == null) return;

        // Initialize if not done yet
        if (!_isInitialized)
        {
            _originalColor = _tmp.color;
            _originalFontSize = _tmp.fontSize;
            _isInitialized = true;
        }

        // Update text
        _tmp.text = $"Try left : {currentTryAmount.ToString()}";

        // Animate: font size up + red color, then font size back + original color
        AnimateTextUpdate();
    }

    //===================================================================== Local Methods
    private void AnimateTextUpdate()
    {
        if (_tmp == null) return;

        // Kill only existing tweens with this instance's ID
        DOTween.Kill(_tweenId);

        // Create sequence: font size up + red color, then font size back + original color
        Sequence sequence = DOTween.Sequence().SetId(_tweenId);

        // Step 1: Font size up and change to red
        sequence.Append(DOTween.To(() => _tmp.fontSize, x => _tmp.fontSize = x, _originalFontSize * 1.2f, 0.2f).SetEase(Ease.OutQuad).SetId(_tweenId));
        sequence.Join(_tmp.DOColor(Color.red, 0.2f).SetId(_tweenId));

        // Step 2: Font size back and restore original color
        sequence.Append(DOTween.To(() => _tmp.fontSize, x => _tmp.fontSize = x, _originalFontSize, 0.2f).SetEase(Ease.OutQuad).SetId(_tweenId));
        sequence.Join(_tmp.DOColor(_originalColor, 0.2f).SetId(_tweenId));
    }
}
