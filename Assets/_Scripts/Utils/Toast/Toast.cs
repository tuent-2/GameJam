using System.Collections.Generic;
using Sirenix.OdinInspector;
using strange.extensions.signal.impl;
using UnityEngine;

public class Toast : MonoBehaviour
{
    [SerializeField] private ToastItem prefabItem;
    [SerializeField] private RectTransform contentHolder;
    private static readonly Signal<ToastData, float, float, bool> _OnShowToast = new();
    private static readonly Signal<string, float, float, bool> _OnShowToastString = new();
    public static readonly Signal<ToastItem> OnAddToast = new();
    public static readonly Signal<ToastItem> OnRemoveToast = new();
    private readonly List<ToastItem> _instances = new();

    private void OnEnable()
    {
        _OnShowToast.AddListener(_Show);
        _OnShowToastString.AddListener(_ShowString);
        OnAddToast.AddListener(AddToast);
        OnRemoveToast.AddListener(RemoveToast);
    }

    private void OnDisable()
    {
        _OnShowToast.RemoveListener(_Show);
        _OnShowToastString.RemoveListener(_ShowString);
        OnAddToast.RemoveListener(AddToast);
        OnRemoveToast.RemoveListener(RemoveToast);
    }

    public static void Show(string str, string prependText = "", string appendText = "", float timeShow = 2f,
        float showPosY = 0,
        bool isClearOther = false)
    {
        _OnShowToast.Dispatch(new ToastData(str, prependText, appendText), timeShow, showPosY, isClearOther);
    }

    public static void ShowString(string str, float timeShow = 2f, float showPosY = 0, bool isClearOther = false)
    {
        _OnShowToastString.Dispatch(str, timeShow, showPosY, isClearOther);
    }

    private void _Show(ToastData data, float timeShow = 2f, float showPosY = 0, bool isClearOther = false)
    {
        if (isClearOther)
        {
            RemoveOtherToast();
        }


        var item = Pool.Get(prefabItem, contentHolder);
        item.SetTimeShow(timeShow).SetStartPosY(showPosY).SetText(data.message, data.prependText, data.appendText)
            .ShowToast();
    }

    private void _ShowString(string str, float timeShow = 2f, float showPosY = 0, bool isClearOther = false)
    {
        if (isClearOther)
        {
            RemoveOtherToast();
        }

        var item = Pool.Get(prefabItem, contentHolder);
        item.SetTimeShow(timeShow).SetStartPosY(showPosY).SetStringText(str).ShowToast();
    }

    private void AddToast(ToastItem item)
    {
        _instances.Add(item);
    }

    private void RemoveToast(ToastItem item)
    {
        _instances.Remove(item);
    }

    private void RemoveOtherToast()
    {
        for (var i = _instances.Count - 1; i >= 0; i--)
        {
            Pool.Release(_instances[i]);
        }
    }

    [Button]
    public void TestShowToast(string message)
    {
        _ShowString(message);
    }
}

public class ToastData
{
    public readonly string message;
    public readonly string prependText;
    public readonly string appendText;

    public ToastData(string message, string prependText, string appendText)
    {
        this.message = message;
        this.prependText = prependText;
        this.appendText = appendText;
    }
}