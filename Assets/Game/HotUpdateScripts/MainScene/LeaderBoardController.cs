using System;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoardController : MonoBehaviour
{
    [SerializeField] private Button btnStartPlay;
    private void Awake()
    {
        Debug.Log(LoginModel.Instance.needUpdate);
        if (LoginModel.Instance.needUpdate)
        {
            ChonLopPopup.Open();
        }
       
    }

    private void Start()
    {
        btnStartPlay.onClick.AddListener(OnClickStartPlay);
    }

    private void OnClickStartPlay()
    {
        
    }
}
