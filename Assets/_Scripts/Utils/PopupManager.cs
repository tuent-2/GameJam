using System;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    private static readonly Dictionary<string, BasePopup> PopupDictionary = new();
    private static readonly Dictionary<string, BasePopup> PopupPrefabDictionary = new();
    private static Transform _cacheTransform;
    [SerializeField] private List<BasePopup> popupPrefabs;

    private void Awake()
    {
        _cacheTransform = transform;
        PopupPrefabDictionary.Clear();
        foreach (var popup in popupPrefabs)
        {
            PopupPrefabDictionary[popup.GetType().Name] = popup;
        }
    }

    private void OnDestroy()
    {
        _cacheTransform = null;
        PopupDictionary.Clear();
        PopupPrefabDictionary.Clear();
    }

    public static void Open<T>(Action<T> onOpen = null) where T : BasePopup<T>
    {
        var key = typeof(T).Name;
        if (PopupDictionary.TryGetValue(key, out var popup))
        {
            popup.OnOpen();
            onOpen?.Invoke(PopupDictionary[key] as T);
        }
        else if (PopupPrefabDictionary.TryGetValue(key, out var prefab))
        {
            var popupView = Instantiate(prefab, _cacheTransform);
            popupView.OnOpen();
            onOpen?.Invoke(popupView as T);
            PopupDictionary[key] = popupView;
        }
        else
        {
            Logger.LogError($"List of Prefabs don't contain popup of type: {key}");
        }
    }

    public static void OpenCreateNew<T>(Action<T> onOpen = null) where T : BasePopup<T>
    {
        var key = typeof(T).Name;


        if (PopupPrefabDictionary.TryGetValue(key, out var prefab))
        {
            var popupView = Instantiate(prefab, _cacheTransform);
            popupView.OnOpen();
            onOpen?.Invoke(popupView as T);

            // Nếu muốn tracking popup đang mở, vẫn lưu vào Dictionary
            if (PopupDictionary.ContainsKey(key))
            {
                Destroy(PopupDictionary[key].gameObject); // Huỷ cái cũ nếu có
                PopupDictionary[key] = popupView;
            }
            else
            {
                PopupDictionary.Add(key, popupView);
            }
        }
        else
        {
            Logger.LogError($"List of Prefabs don't contain popup of type: {key}");
        }
    }


    public static bool IsOpen<T>()
    {
        var key = typeof(T).Name;
        return PopupDictionary.TryGetValue(key, out var popup) && popup.IsOpening;
    }

    public static void Close<T>()
    {
        var key = typeof(T).Name;
        if (PopupDictionary.TryGetValue(key, out var popup))
        {
            popup.OnClose();
        }
    }

    public static void CloseAllPopup()
    {
        foreach (var popup in PopupDictionary.Values)
        {
            popup.OnClose();
        }
    }
}