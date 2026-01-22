using System;
using System.Collections;
using DG.Tweening;

using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class FocusItem : MonoBehaviour
{
    [SerializeField] private GameObject goContent;
    [SerializeField] private RectTransform goCharPosition;
    [SerializeField] private RectTransform holderTrs;
    [SerializeField] private RectTransform rightTrs, leftTrs, upTrs, downTrs;

    [SerializeField] private RectTransform focusTrs;

    //[SerializeField] private RectTransform bgTrs;
    [SerializeField] private float durationSizeDelta = 0.4f;

    [SerializeField] private Vector2 sizeOffset = new Vector2(20, 20);
    public RectTransform FocusTrs => focusTrs;

     public RectTransform target;
    [HideInInspector] public bool isGenGamePlayedDone = false;
    [HideInInspector] public bool isGenListGameDone = false;

    private Tween _tweenShow;

    private bool _isTutorialBegin;
    // private TutorialStep _step;
    // private TutorialBeginGameStep _stepBegin;

    [SerializeField] private TextMeshProUGUI txtContent;

    //[SerializeField] private RectTransform testTarget;
    private string[] lines;


    [SerializeField] float textSpeed;
    [SerializeField] ForcusItem2 item2;
    private int index;
    

    private void Start()
    {
        txtContent.text = string.Empty;
    }

    void StartText()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        lines = new string[]
        {
            "Chào mừng đ"
        };

        foreach (char c in lines[index].ToCharArray())
        {
            txtContent.text += c;
            yield return new WaitForSeconds(textSpeed / 2);
        }

        // Chờ 1 chút rồi chuyển sang dòng tiếp theo
        yield return new WaitForSeconds(0.5f);
        NextLine();
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            // Thêm xuống dòng trước khi in line mới
            txtContent.text += "\n";
            StartCoroutine(TypeLine());
        }
        // else
        // {
        //     // Hết câu cuối -> ẩn đối tượng
        //     gameObject.SetActive(false);
        // }
    }


    public static FocusItem Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void HideView()
    {
        goContent.SetActive(false);
    }

    [Button]
    public void ShowView2()
    {
        PopupManager.CloseAllPopup();
        txtContent.text = string.Empty;
        //bgTrs.Show();
        goContent.Show();
        _isTutorialBegin = false;

        var position = target.position;
        focusTrs.position = position;
        goCharPosition.position = position;
        focusTrs.sizeDelta = target.sizeDelta + Vector2.one * 40;
        _tweenShow?.Kill();
        _tweenShow = ShowViews(position, target.sizeDelta + Vector2.one * 40).OnComplete(() =>
        {
            item2.Show();
           // StartText();
        });

        //var handler = target.gameObject.AddComponent<TutorialClickHandler>();
        //handler.SetClickAction(MarkComplete);
    }

    // public void ShowView(RectTransform target, TutorialBeginGameStep step)
    // {
    //     bgTrs.Show();
    //     gameObject.Show();
    //     _isTutorialBegin = false;
    //     _stepBegin = step;
    //     var position = target.position;
    //     focusTrs.position = position;
    //     focusTrs.sizeDelta = target.sizeDelta + Vector2.one * 40;
    //     _tweenShow?.Kill();
    //     _tweenShow = ShowViews(position, target.sizeDelta + Vector2.one * 40);
    //
    //     var handler = target.gameObject.AddComponent<TutorialClickHandler>();
    //     handler.SetClickAction(MarkCompleteTutBeginGame);
    // }

    public void ShowView(RectTransform target)
    {
        focusTrs.position = target.position;
        focusTrs.sizeDelta = target.sizeDelta + Vector2.one * 30;
        UpdateViewByCover();
    }

    [Button]
    private void UpdateViewByCover()
    {
        var mainSize = holderTrs.rect;
        var w = mainSize.width / 2;
        var h = mainSize.height / 2;
        var deltaSize = focusTrs.sizeDelta;
        var focusPos = focusTrs.anchoredPosition;
        var rightWidth = w - focusPos.x - deltaSize.x / 2;
        var leftWidth = w + focusPos.x - deltaSize.x / 2;
        var upHeight = h - focusPos.y - deltaSize.y / 2;
        var downHeight = h + focusPos.y - deltaSize.y / 2;
        rightTrs.ChangeSizeX(rightWidth);
        leftTrs.ChangeSizeX(leftWidth);

        upTrs.SetLeft(leftWidth);
        upTrs.SetRight(rightWidth);
        upTrs.ChangeSizeY(upHeight);

        downTrs.SetLeft(leftWidth);
        downTrs.SetRight(rightWidth);
        downTrs.ChangeSizeY(downHeight);
    }


    public Tween ShowViews(Vector3 position, Vector2 pivotDeltaSize)
    {
        focusTrs.position = position;
        focusTrs.sizeDelta = new Vector2(Screen.width + 200, Screen.height + 200);
        UpdateViewByCover();
        return focusTrs.DOSizeDelta(pivotDeltaSize, durationSizeDelta, false).OnUpdate(UpdateViewByCover)
            .OnComplete(() => { });
    }

    public Tween CloseToComplete()
    {
        var pivotDeltaSize = new Vector2(Screen.width + 200, Screen.height + 800);
        return focusTrs.DOSizeDelta(pivotDeltaSize, durationSizeDelta, false).OnUpdate(UpdateViewByCover)
            .OnComplete(() => { });
        ;
    }

    // private void MarkComplete()
    // {
    //     //_step.MarkComplete();
    //     gameObject.Hide();
    // }
    //
    // private void MarkCompleteTutBeginGame()
    // {
    //     //_stepBegin.MarkComplete();
    //     gameObject.Hide();
    // }
}