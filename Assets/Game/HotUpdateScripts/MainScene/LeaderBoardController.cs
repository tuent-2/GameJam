using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoardController : MonoBehaviour
{
    [SerializeField] private Button btnStartPlay;

    [SerializeField] private List<Character> _listChar;

    private void Awake()
    {
        Debug.Log(LoginModel.Instance.needUpdate);
        if (LoginModel.Instance.needUpdate)
        {
            ChonLopPopup.Open();
        }

        GameControllerModel.Instance.SendGetLeaderBoard();

    }

    private void OnEnable()
    {
        GameControllerModel.Instance.leaderBoardResponse.OnChanged += OnChanged;
    }

    private void OnDisable()
    {
        GameControllerModel.Instance.leaderBoardResponse.OnChanged -= OnChanged;
    }

    private void Start()
    {
        btnStartPlay.onClick.AddListener(OnClickStartPlay);
        StartCoroutine(InitializeLeaderBoard());
    }

    private IEnumerator InitializeLeaderBoard()
    {
        OnChanged();
        var value = GameControllerModel.Instance.leaderBoardResponse.Value;

        int maxRetries = 600; // Tối đa 60 frame (khoảng 1 giây nếu 60fps)
        int retryCount = 0;

        while (value == null && retryCount < maxRetries)
        {
           // Debug.Log("Wait 1 frame");
            yield return null; // Đợi 1 frame
            value = GameControllerModel.Instance.leaderBoardResponse.Value;
            retryCount++;
        }

        if (value != null)
        {
            OnChanged();
        }
        else
        {
            Debug.LogWarning("LeaderBoard data is still null after maximum retries");
        }
    }

    private void OnClickStartPlay()
    {
        StartPlayPopup.Open();
    }

    private void OnChanged()
    {
        var value = GameControllerModel.Instance.leaderBoardResponse.Value;

        if (value?.LeaderBoards == null || _listChar == null)
            return;

        // Update characters with leaderboard data (up to the minimum of both counts)
        int updateCount = Mathf.Min(value.LeaderBoards.Count, _listChar.Count);
        for (int i = 0; i < updateCount; i++)
        {
            _listChar[i].UpdateVisualByData(value.LeaderBoards[i]);
        }

        // For characters beyond the leaderboard count, check if they need to be reset
        // Note: Since LeaderBoard doesn't have a parameterless constructor,
        // we skip characters that weren't updated in the loop above
        for (int i = updateCount; i < _listChar.Count; i++)
        {
            if (_listChar[i].data == null)
            {
                _listChar[i].FakeVisual();
            }
        }
    }

    [Button]
    private void GetCamelRef()
    {
        _listChar = new List<Character>(GetComponentsInChildren<Character>());
    }
}