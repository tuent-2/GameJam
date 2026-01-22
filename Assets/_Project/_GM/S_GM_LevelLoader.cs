using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PTT;

public class S_GM_LevelLoader : GMModuleBehaviour
{
    //===================================================================== Variables
    public List<Level> listLevelPrefab;

    [SerializeField] private Transform _levelRoot;
    private Level _currentLevelInstance;
    private Coroutine _spawnRoutine;
    //===================================================================== Unity Methods
    private void Start()
    {
        // Spawn current level after 1 frame (legacy behavior)
        ReloadCurrentLevel(waitOneFrame: true);
    }

    //===================================================================== Action Handle

    //===================================================================== Local Methods
    public void ReloadCurrentLevel(bool waitOneFrame = false)
    {
        if (_spawnRoutine != null)
        {
            StopCoroutine(_spawnRoutine);
            _spawnRoutine = null;
        }

        _spawnRoutine = StartCoroutine(SpawnCurrentLevelRoutine(waitOneFrame));
    }

    private IEnumerator SpawnCurrentLevelRoutine(bool waitOneFrame)
    {
        if (waitOneFrame)
        {
            // Wait 1 frame after Start (per request)
            yield return null;
        }

        if (listLevelPrefab == null || listLevelPrefab.Count == 0)
        {
            Debug.LogError("S_GM_LevelLoader -> listLevelPrefab is null/empty.");
            _spawnRoutine = null;
            yield break;
        }

        var index = ProjectDataDynamic.Instance?.iCurrentLevel ?? 0;
        index = Mathf.Clamp(index, 0, listLevelPrefab.Count - 1);

        var levelPrefab = listLevelPrefab[index];
        if (levelPrefab == null)
        {
            Debug.LogError($"S_GM_LevelLoader -> level prefab at index {index} is null.");
            _spawnRoutine = null;
            yield break;
        }

        // Cleanup old level (if any)
        if (_currentLevelInstance != null)
        {
            // Unregister previous level module so GM doesn't keep stale references.
            GM.Instant.UntrackModuleBehaviour(_currentLevelInstance, dispose: true);
            Destroy(_currentLevelInstance.gameObject);
            _currentLevelInstance = null;
        }

        var parent = _levelRoot != null ? _levelRoot : transform;
        _currentLevelInstance = Instantiate(levelPrefab, parent);
        _currentLevelInstance.name = levelPrefab.name;

        // IMPORTANT: register spawned level as a GM module so other scripts can resolve it via GM.Instant.GetModule<Level>().
        // Also pushes the reference into GM.moduleBehaviours (Assets/PTTSharedLibrary/Core/GM.cs:29) for inspection/debugging.
        GM.Instant.TrackModuleBehaviour(_currentLevelInstance, initialize: true);

        // Wait until "load done" (instantiate is sync, but give 1 frame for Awake/Start on the level)
        yield return null;

        // Switch state to Playing
        var gmState = GM.Instant.GetModule<S_GM_State>();
        if (gmState != null)
        {
            gmState.eGameStateCurrent = EGameState.Playing;
        }
        else
        {
            Debug.LogWarning("S_GM_LevelLoader -> S_GM_State module not found, cannot set Playing.");
        }

        _spawnRoutine = null;
    }
}
