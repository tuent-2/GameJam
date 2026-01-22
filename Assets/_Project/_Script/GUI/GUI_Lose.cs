using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using PTT;
using DG.Tweening;

public class GUI_Lose : ExtendMonoBehaviour
{
    //===================================================================== Variables
    [SerializeField] private string _sSceneName;
    [SerializeField] private Button _btnRetry;
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

		if (_btnRetry != null)
		{
			_btnRetry.onClick.AddListener (OnClickRetry);
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
		if (_btnRetry != null)
		{
			_btnRetry.onClick.RemoveListener (OnClickRetry);
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
		if (toState == EGameState.EndLose)
		{
			// Re-active object when state changes to EndLose
			ShowWithAnimation();
		}
	}

	private void OnClickRetry ()
	{
		var levelLoader = GM.Instant.GetModule<S_GM_LevelLoader> ();
		if (levelLoader != null)
		{
			// Hide lose UI with animation, then reload the current level via LevelLoader
			HideWithAnimation(() => {
				levelLoader.ReloadCurrentLevel ();
			});
			return;
		}

		// Fallback: reload the active scene if the module isn't available
		HideWithAnimation(() => {
			var activeScene = SceneManager.GetActiveScene ();
			SceneManager.LoadScene (activeScene.buildIndex);
		});
	}

	private void OnClickLeave ()
	{
		SceneManager.UnloadSceneAsync(_sSceneName);
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
