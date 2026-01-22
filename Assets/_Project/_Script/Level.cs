using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Serialization;
using PTT;
using Sirenix.OdinInspector;
using TMPro;

public class Level : GMModuleBehaviour
{
    //===================================================================== Variables
    public int iMaxTryAmount;
    [DisableIn(PrefabKind.All)]
    [SerializeField] private int _iCurrentTryAmount;

    [SerializeField] private TextMeshPro _tmpWeightIncluse;
    private int _weightIncluseSignature;
    
    protected S_GM_Event _gmEvent;
    protected S_GM_State _gmState;

    public int iCurrentTryAmount
    {
        get => _iCurrentTryAmount;
        set => SetCurrentTryAmount(value);
    }

    public List<Ball> listBallRef;
    //===================================================================== Unity Methods
    protected override void Awake()
    {
        base.Awake();
        iCurrentTryAmount = iMaxTryAmount;
        RefreshWeightIncluseText(force: true);
        
        // Hoán đổi vị trí của tất cả ball trong listBallRef
        if (listBallRef != null && listBallRef.Count > 1)
        {
            // Lưu tất cả vị trí hiện tại
            List<Vector3> positions = new List<Vector3>(listBallRef.Count);
            for (int i = 0; i < listBallRef.Count; i++)
            {
                if (listBallRef[i] != null)
                {
                    positions.Add(listBallRef[i].transform.position);
                }
            }
            
            // Xáo trộn danh sách vị trí
            for (int i = positions.Count - 1; i > 0; i--)
            {
                int randomIndex = UnityEngine.Random.Range(0, i + 1);
                Vector3 temp = positions[i];
                positions[i] = positions[randomIndex];
                positions[randomIndex] = temp;
            }
            
            // Gán lại vị trí đã xáo trộn cho các ball
            for (int i = 0; i < listBallRef.Count && i < positions.Count; i++)
            {
                if (listBallRef[i] != null)
                {
                    listBallRef[i].transform.position = positions[i];
                }
            }
        }
    }

    protected virtual void Start()
    {
        StartCoroutine(SubscribeEventsNextFrame());
    }

    private void LateUpdate()
    {
        // Keep text in sync at runtime if list/weights are changed by gameplay.
        RefreshWeightIncluseText(force: false);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        RefreshWeightIncluseText(force: true);
    }
#endif
    private IEnumerator SubscribeEventsNextFrame()
    {
        yield return null; // wait 1 frame after Start

        _gmEvent = GM.Instant.GetModule<S_GM_Event>();
        if (_gmEvent != null)
        {
            _gmEvent.OnCurrentTryAmountChange += HandleCurrentTryAmountChange;
        }
    }

    private void OnDestroy()
    {
        if (_gmEvent != null)
        {
            _gmEvent.OnCurrentTryAmountChange -= HandleCurrentTryAmountChange;
        }
        _gmEvent = null;
        _gmState = null;
    }
    //===================================================================== Action Handle
    protected virtual void HandleCurrentTryAmountChange(int currentTryAmount)
    {

    }

    //===================================================================== Local Methods
    private void SetCurrentTryAmount(int value)
    {
        if (_iCurrentTryAmount == value) return;
        _iCurrentTryAmount = value;

        // Broadcast via GM event module (safe even if module not present).
        GM.Instant.GetModule<S_GM_Event>()?.OnCurrentTryAmountChange?.Invoke(_iCurrentTryAmount);
    }

    private void RefreshWeightIncluseText(bool force)
    {
        if (_tmpWeightIncluse == null) return;

        int sig = ComputeWeightIncluseSignature();
        if (!force && sig == _weightIncluseSignature) return;

        _weightIncluseSignature = sig;
        _tmpWeightIncluse.text = BuildWeightIncluseText();
    }

    private int ComputeWeightIncluseSignature()
    {
        unchecked
        {
            int hash = 17;
            if (listBallRef == null) return hash;

            hash = hash * 31 + listBallRef.Count;
            for (int i = 0; i < listBallRef.Count; i++)
            {
                var b = listBallRef[i];
                hash = hash * 31 + (b != null ? b.GetInstanceID() : 0);
                if (b == null) continue;

                // Match UI precision (0.##) => round to 2 decimals for stable signature.
                int scaled = Mathf.RoundToInt(b.Weight * 100f);
                hash = hash * 31 + scaled;
            }
            return hash;
        }
    }

    private string BuildWeightIncluseText()
    {
        if (listBallRef == null || listBallRef.Count == 0) return string.Empty;

        // Unique weights, sorted, formatted like Ball's TMP ("0.##").
        var seen = new HashSet<int>();
        var scaledWeights = new List<int>(listBallRef.Count);

        for (int i = 0; i < listBallRef.Count; i++)
        {
            var b = listBallRef[i];
            if (b == null) continue;

            int scaled = Mathf.RoundToInt(b.Weight * 100f);
            if (seen.Add(scaled)) scaledWeights.Add(scaled);
        }

        if (scaledWeights.Count == 0) return string.Empty;

        scaledWeights.Sort();

        var sb = new StringBuilder(32);
        for (int i = 0; i < scaledWeights.Count; i++)
        {
            if (i > 0) sb.Append(", ");
            float w = scaledWeights[i] / 100f;
            sb.Append(w.ToString("0.##"));
        }

        return $"Ball Incluse : {sb.ToString()}";
    }

    [Button]
    private void FindBallRef()
    {
        if (listBallRef == null) listBallRef = new List<Ball>();
        listBallRef.Clear();

        var targetScene = gameObject.scene;
        bool filterByScene = targetScene.IsValid();

        // NOTE:
        // - FindObjectsOfType/FindObjectsByType can be unreliable when the button is pressed on a prefab asset
        //   (Project window inspector / preview object). To make this work consistently in Scene, Prefab Stage,
        //   and prefab asset preview, we collect candidates via hierarchy traversal instead.
        var candidates = new List<Ball>(32);
        if (filterByScene)
        {
            var roots = targetScene.GetRootGameObjects();
            for (int r = 0; r < roots.Length; r++)
            {
                if (roots[r] == null) continue;
                candidates.AddRange(roots[r].GetComponentsInChildren<Ball>(true));
            }
        }
        else
        {
            // Fallback: search inside this prefab instance only.
            candidates.AddRange(GetComponentsInChildren<Ball>(true));
        }

        for (int i = 0; i < candidates.Count; i++)
        {
            var b = candidates[i];
            if (b == null) continue;

            // Requirement: only take GameObjects that are active.
            // - In scenes/prefab stage: use activeInHierarchy (matches runtime behaviour).
            // - In prefab asset preview (no valid scene): use activeSelf.
            bool isActive = filterByScene ? b.gameObject.activeInHierarchy : b.gameObject.activeSelf;
            if (!isActive) continue;

            if (filterByScene && b.gameObject.scene != targetScene) continue; // only this scene (if valid)
            listBallRef.Add(b);
        }

        // stable ordering for easier inspection/debugging
        listBallRef.Sort((a, b) =>
        {
            if (a == b) return 0;
            if (a == null) return 1;
            if (b == null) return -1;
            return string.CompareOrdinal(GetHierarchyPath(a.transform), GetHierarchyPath(b.transform));
        });

        static string GetHierarchyPath(Transform t)
        {
            if (t == null) return string.Empty;

            // Build "Root/Child/..." path
            var stack = new Stack<string>(8);
            while (t != null)
            {
                stack.Push(t.name);
                t = t.parent;
            }

            return string.Join("/", stack);
        }

#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
        if (!Application.isPlaying)
        {
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(gameObject.scene);
        }
#endif

        RefreshWeightIncluseText(force: true);
    }
}
