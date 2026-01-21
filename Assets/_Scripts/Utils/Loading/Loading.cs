using System;
using System.Collections;
using DG.Tweening;
using strange.extensions.signal.impl;
using UnityEngine;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    [SerializeField] private LoadingItem loadingItem;
    public static Signal<bool> OnShowLoading = new();

    private void OnEnable()
    {
        OnShowLoading.AddListener(ShowLoading);
    }

    private void OnDisable()
    {
        OnShowLoading.RemoveListener(ShowLoading);
    }

    private void ShowLoading(bool isShow)
    {
        if (isShow)
        {
            loadingItem.OnOpenLoading();
        }
        else
        {
            loadingItem.OnCloseLoading();
        }
    }


    public static void Open()
    {
        Logger.Log("Open Loading");
        OnShowLoading.Dispatch(true);
    }

    public static void Close()
    {
        Logger.Log("Close Loading");
        OnShowLoading.Dispatch(false);
    }
}