using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LeaderBoardController : MonoBehaviour
{
    [SerializeField] private Button btnStartPlay_1v1;
    [SerializeField] private Button btnStartPlay_EzWeight;
    [SerializeField] private Button btnStartPlay_HardWeight;
    [SerializeField] private Button btnStartPlay, btnThanhTich;

    [SerializeField] private List<Character> _listChar;

    private void Awake()
    {
        Debug.Log(LoginModel.Instance.needUpdate);
        if (LoginModel.Instance.needUpdate)
        {
            ChonLopPopup.Open();
        }
    }

    [Button]
    public void SendGetLeaderBoard()
    {
        GameControllerModel.Instance.SendGetLeaderBoard();
    }

    private void OnEnable()
    {
        StartCoroutine(Init());
       
    }

    private IEnumerator Init()
    {
        yield return new WaitForEndOfFrame();
        SendGetLeaderBoard();
        GameControllerModel.Instance.leaderBoardResponse.OnChanged += OnChanged;
    }

    private void OnDisable()
    {
        GameControllerModel.Instance.leaderBoardResponse.OnChanged -= OnChanged;
    }

    private void Start()
    {
        btnStartPlay_1v1.onClick.AddListener(OnClickStartPlay_1v1);
        btnStartPlay_EzWeight.onClick.AddListener(OnClickStartPlay_EzWeight);
        btnStartPlay_HardWeight.onClick.AddListener(OnClickStartPlay_HardWeight);  
        StartCoroutine(InitializeLeaderBoard());
        btnThanhTich.onClick.AddListener(OnClickThanhTich);
    }

    private void OnClickThanhTich()
    {
        PersonalInfoPopup.Open();
    }

    private void OnClickStartPlay_EzWeight()
    {
        SceneManager.LoadSceneAsync("Level_Balance",LoadSceneMode.Additive);
    }

    private void OnClickStartPlay_HardWeight()
    {
        SceneManager.LoadSceneAsync("Level_FindOne",LoadSceneMode.Additive);
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

    private void OnClickStartPlay_1v1()
    {
        StartPlayPopup.Open();
    }

    private void OnChanged()
    {
        var value = GameControllerModel.Instance.leaderBoardResponse.Value;

        if (value?.LeaderBoards == null || _listChar == null)
            return;

        for (int i = 0; i < _listChar.Count; i++)
        {
            if (i < value.LeaderBoards.Count)
                _listChar[i].UpdateVisualByData(value.LeaderBoards[i]);
            else
                _listChar[i].FakeVisual();
        }
    }

    [Button]
    private void GetCamelRef()
    {
        _listChar = new List<Character>(GetComponentsInChildren<Character>());
    }
}