using System.Collections.Generic;
using UnityEngine;

public static class Helpers
{
    private static readonly Dictionary<float, WaitForSeconds> WaitDictionary = new Dictionary<float, WaitForSeconds>();

    private static readonly Dictionary<float, WaitForSecondsRealtime> WaitRealTimeDictionary =
        new Dictionary<float, WaitForSecondsRealtime>();

    public static WaitForSeconds GetWaitForSeconds(float time)
    {
        if (WaitDictionary.TryGetValue(time, out var wait)) return wait;

        WaitDictionary[time] = new WaitForSeconds(time);
        return WaitDictionary[time];
    }

    public static WaitForSecondsRealtime GetWaitForSecondsRealTime(float time)
    {
        if (WaitRealTimeDictionary.TryGetValue(time, out var wait)) return wait;

        WaitRealTimeDictionary[time] = new WaitForSecondsRealtime(time);
        return WaitRealTimeDictionary[time];
    }


    public static void DeleteChildren(this Transform t)
    {
        foreach (Transform child in t) Object.Destroy(child.gameObject);
    }
}