using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Pool : MonoSingleton<Pool>
{
    private Dictionary<string, ObjectPool<MonoBehaviour>> _poolDictionary = new();

    private MonoBehaviour CreateObject(MonoBehaviour prefab, Transform parent)
    {
        var instantiatedPrefab = Instantiate(prefab, parent ? parent : Instance.transform);
        return instantiatedPrefab;
    }


    private void OnGetObject(MonoBehaviour item)
    {
        item.gameObject.SetActive(true);
    }

    private void OnReleaseObject(MonoBehaviour item)
    {
        item.gameObject.SetActive(false);
    }

    private void OnDestroyObject(MonoBehaviour item)
    {
        Destroy(item.gameObject);
    }

    public static void Release(MonoBehaviour item, string variant = "")
    {
        var key = item.GetType().Name + variant;
        if (Instance._poolDictionary.ContainsKey(key))
            Instance._poolDictionary[key].Release(item);
    }

    public static T Get<T>(T prefab, Transform parent = null, string variant = "") where T : MonoBehaviour
    {
        var key = prefab.GetType().Name + variant;
        if (!Instance._poolDictionary.ContainsKey(key))
        {
            Instance._poolDictionary[key] = new ObjectPool<MonoBehaviour>(
                () => Instance.CreateObject(prefab, parent), Instance.OnGetObject,
                Instance.OnReleaseObject,
                Instance.OnDestroyObject, false,
                100);
        }

        var item = (T)Instance._poolDictionary[key].Get();
        item.transform.SetParent(parent ? parent : Instance.transform);
        return item;
    }

    public static T Get<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent = null,
        bool isLocal = false, string variant = "")
        where T : MonoBehaviour
    {
        var key = prefab.GetType().Name + variant;
        if (!Instance._poolDictionary.ContainsKey(key))
        {
            Instance._poolDictionary[key] = new ObjectPool<MonoBehaviour>(
                () => Instance.CreateObject(prefab, parent), Instance.OnGetObject,
                Instance.OnReleaseObject,
                Instance.OnDestroyObject, false,
                1000);
        }

        var item = (T)Instance._poolDictionary[key].Get();
        if (parent)
            item.transform.SetParent(parent);
        if (isLocal)
        {
            item.transform.SetLocalPositionAndRotation(position, rotation);
        }
        else
        {
            item.transform.SetPositionAndRotation(position, rotation);
        }

        return item;
    }

    public static void ClearAll()
    {
        foreach (var item in Instance._poolDictionary.Values)
        {
            item.Dispose();
        }

        /*foreach (Transform trans in Instance.transform)
        {
            Destroy(trans.gameObject);
        }*/

        Instance._poolDictionary.Clear();
    }
}