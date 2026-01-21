using System;
using UnityEngine;

public class LeaderBoardController : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log(LoginModel.Instance.needUpdate);
        if (LoginModel.Instance.needUpdate)
        {
            ChonLopPopup.Open();
        }
       
    }
}
