using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public static class ExtensionFunction
{
    public static void PlayRandomClips(this AudioClip[] audioClips, float volumeScale = 1f)
    {
        AudioManager.PlaySound(audioClips[Random.Range(0, audioClips.Length)], volumeScale);
    }

    public static void PlayRandomClips(this List<AudioClip> audioClips, float volumeScale = 1f)
    {
        AudioManager.PlaySound(audioClips[Random.Range(0, audioClips.Count)], volumeScale);
    }

    public static void Move<T>(this List<T> list, int oldIndex, int newIndex)
    {
        var item = list[oldIndex];

        list.RemoveAt(oldIndex);
        list.Insert(newIndex, item);
    }

    public static void Move<T>(this List<T> list, T item, int newIndex)
    {
        if (item != null)
        {
            var oldIndex = list.IndexOf(item);
            if (oldIndex > -1)
            {
                list.RemoveAt(oldIndex);
                list.Insert(newIndex, item);
            }
        }
    }

    public static void Hide(this GameObject go)
    {
        if (!go) return;
        go.SetActive(false);
    }

    public static void Show(this GameObject go)
    {
        if (!go) return;
        go.SetActive(true);
    }

    public static void Hide(this MonoBehaviour go)
    {
        if (!go) return;
        go.gameObject.SetActive(false);
    }

    public static void Show(this MonoBehaviour go)
    {
        if (!go) return;
        go.gameObject.SetActive(true);
    }

    public static void Hide(this Component go)
    {
        if (!go) return;
        go.gameObject.SetActive(false);
    }

    public static void Show(this Component go)
    {
        if (!go) return;
        go.gameObject.SetActive(true);
    }

    public static void ChangeAlpha(this Graphic graphic, float a)
    {
        var color = graphic.color;
        color.a = a;
        graphic.color = color;
    }

    public static void ChangeAlpha(this Material material, float a)
    {
        var color = material.color;
        color.a = a;
        material.color = color;
    }

    public static void ChangeAnchorX(this RectTransform rect, float x)
    {
        var pos = rect.anchoredPosition;
        pos.x = x;
        rect.anchoredPosition = pos;
    }

    public static void ChangeAnchorY(this RectTransform rect, float y)
    {
        var pos = rect.anchoredPosition;
        pos.y = y;
        rect.anchoredPosition = pos;
    }

    public static void ChangeSizeX(this RectTransform rect, float x)
    {
        var size = rect.sizeDelta;
        size.x = x;
        rect.sizeDelta = size;
    }

    public static void ChangeSizeY(this RectTransform rect, float y)
    {
        var size = rect.sizeDelta;
        size.y = y;
        rect.sizeDelta = size;
    }

    public static void SetLeft(this RectTransform rt, float left)
    {
        rt.offsetMin = new Vector2(left, rt.offsetMin.y);
    }

    public static void SetRight(this RectTransform rt, float right)
    {
        rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
    }

    public static List<T> Splice<T>(this List<T> list, int offset, int count)
    {
        var startIdx = offset < 0 ? list.Count + offset : offset;
        var result = list.Skip(startIdx).Take(count).ToList();
        list.RemoveRange(startIdx, count);
        return result;
    }

    public static List<T> Split<T>(this List<T> ls, int start, int length)
    {
        if (ls.Count < start + length)
        {
            //SDLogger.LogError("Chiều dài list không đủ");
            return null;
        }

        var l = new List<T>();
        for (int i = start; i < start + length; i++)
        {
            l.Add(ls[i]);
        }

        return l;
    }

    public static void WaitNewFrame(this MonoBehaviour obj, Action callback)
    {
        Executors.RunCoroutineWithoutReturn(IWaitNewFrame(callback));
    }

    private static IEnumerator IWaitNewFrame(Action callBack)
    {
        yield return null;
        callBack?.Invoke();
    }

    public static Coroutine WaitNewFrameWithReturn(this MonoBehaviour obj, Action callback)
    {
        return Executors.RunCoroutineWithReturn(IWaitNewFrame(callback));
    }

    public static List<T> SpliceGetLast<T>([NotNull] this List<T> ls, int count)
    {
        var temp = new List<T>();
        for (int i = 0; i < count; i++)
        {
            var x = ls.Count - (count - i);
            if (x >= 0)
            {
                temp.Add(ls[x]);
            }
        }

        return temp;
    }
}