using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using PTT;
using DG.Tweening;

public class GUI_Win : ExtendMonoBehaviour
{
    //===================================================================== Variables
    [SerializeField] private Button _btnNext;
    [SerializeField] private Button _btnLeave;

	private GM_Event _gmEvent;
	private bool _isSubscribed;
	[SerializeField] private float _scaleAnimDuration = 0.3f;
	[SerializeField] private Ease _scaleEase = Ease.OutBack;
	private Vector3 _originalScale;
    //===================================================================== Unity Methods
	private IEnumerator Start ()
	{
		// Wait 1 frame after Start
		yield return null;

		if (_btnNext != null)
		{
			_btnNext.onClick.AddListener (OnClickNext);
		}

		if (_btnLeave != null)
		{
			_btnLeave.onClick.AddListener (OnClickLeave);
		}

		_gmEvent = GM.Instant.GetModule<GM_Event> ();
		if (_gmEvent != null)
		{
			_gmEvent.OnGameStateChange += HandleGameStateChange;
			_isSubscribed = true;
		}

		// Store original scale
		_originalScale = transform.localScale;
		
		// Disable after registering event
		gameObject.SetActive (false);
	}

	private void OnDestroy ()
	{
		if (_btnNext != null)
		{
			_btnNext.onClick.RemoveListener (OnClickNext);
		}

		if (_btnLeave != null)
		{
			_btnLeave.onClick.RemoveListener (OnClickLeave);
		}

		if (_isSubscribed && _gmEvent != null)
		{
			_gmEvent.OnGameStateChange -= HandleGameStateChange;
		}
	}

    //===================================================================== Action Handle
	private void HandleGameStateChange (EGameState fromState, EGameState toState)
	{
		if (toState == EGameState.EndWin)
		{
			// If this is the last level, hide "Next" button.
			if (_btnNext != null)
			{
				_btnNext.gameObject.SetActive(!IsLastLevel());
			}

			// Re-active object when state changes to EndWin
			ShowWithAnimation();
		}
	}

	private bool IsLastLevel()
	{
		var levelLoader = GM.Instant.GetModule<S_GM_LevelLoader>();
		if (levelLoader == null || levelLoader.listLevelPrefab == null || levelLoader.listLevelPrefab.Count <= 0)
		{
			// No level list info -> treat as last to avoid showing a broken "Next".
			return true;
		}

		var data = ProjectDataDynamic.Instance;
		var index = data != null ? data.iCurrentLevel : 0;
		index = Mathf.Clamp(index, 0, levelLoader.listLevelPrefab.Count - 1);
		return index >= levelLoader.listLevelPrefab.Count - 1;
	}

	private void OnClickNext ()
	{
		// Increase current level index, then reload level via GM module loader.
		var data = ProjectDataDynamic.Instance;
		if (data != null)
		{
			data.iCurrentLevel += 1;
		}
		else
		{
			Debug.LogWarning("GUI_Win -> ProjectDataDynamic.Instance is null. Fallback to level index 0.");
		}

		var levelLoader = GM.Instant.GetModule<S_GM_LevelLoader>();
		if (levelLoader != null)
		{
			// Hide win UI with animation; LevelLoader will set state back to Playing after spawn.
			HideWithAnimation(() => {
				levelLoader.ReloadCurrentLevel(waitOneFrame: false);
			});
			return;
		}

		Debug.LogWarning("GUI_Win -> S_GM_LevelLoader module not found. Fallback to reloading current scene.");
		HideWithAnimation(() => {
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		});
	}

	private void OnClickLeave ()
	{
		// Best-effort: go back to build index 0 if possible, otherwise quit.
		if (SceneManager.sceneCountInBuildSettings > 0)
		{
			SceneManager.LoadScene (0);
			return;
		}

		Application.Quit ();
	}
    
    //===================================================================== Local Methods
	private void ShowWithAnimation()
	{
		gameObject.SetActive(true);
		transform.localScale = Vector3.zero;
		transform.DOKill();
		transform.DOScale(_originalScale, _scaleAnimDuration)
			.SetEase(_scaleEase);
	}

	private void HideWithAnimation(System.Action onComplete = null)
	{
		transform.DOKill();
		transform.DOScale(Vector3.zero, _scaleAnimDuration)
			.SetEase(Ease.InBack)
			.OnComplete(() => {
				gameObject.SetActive(false);
				onComplete?.Invoke();
			});
	}
}
